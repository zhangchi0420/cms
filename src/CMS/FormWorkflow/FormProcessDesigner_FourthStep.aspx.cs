using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessDesigner_FourthStep : BasePage
    {
        #region 属性

        /// <summary>
        /// 流程ID
        /// </summary>
        protected long ProcessId
        {
            get { return VS<long>("ProcessId"); }
            set { VS<long>("ProcessId", value); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Initialize();
                }                
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormProcessQuery.aspx");
            }
        }

        private List<Exception> errorList = new List<Exception>();

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            long? id = QueryString<long?>("id");

            SysProcess process = this.DataHelper.FindById<SysProcess>(id);
            if (process != null)
            {
                this.ProcessId = id.Value;
                if (!string.IsNullOrEmpty(process.ProcessName))
                {
                    if (!string.IsNullOrEmpty(process.ProcessVersion))
                    {
                        BindAndValidateData(process);
                    }
                    else
                    {
                        AddError(new Exception("流程版本内部错误，请重建流程"));
                    }
                }
                else
                {
                    AddError(new Exception("流程名称为空，请返回第一步配置"));
                }                
            }
            else
            {
                AddError(new Exception("流程不存在"));
            }
            this.btnPublish.Visible = errorList.Count == 0;

            this.gcError.DataSource = errorList;
            this.gcError.DataBind();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        private void AddError(Exception ex)
        {
            if (errorList == null)
            {
                errorList = new List<Exception>();
            }
            errorList.Add(ex);
        }

        private string GetActivityParticipant(SysActivity a)
        {
            var source = from ap in this.DataHelper.Set<SysActivityParticipant>()
                         join pp in this.DataHelper.Set<SysProcessParticipant>()
                         on ap.ParticipantId equals pp.ParticipantId
                         where ap.ActivityId == a.ActivityId
                         select pp.ParticipantName;
            string result = string.Join("、", source.ToList());
            return result;
        }

        /// <summary>
        /// 验证流程的完整性，绑定流程详情和验证错误信息
        /// </summary>
        private void BindAndValidateData(SysProcess process)
        {
            this.lblProcessName.Text = process.ProcessName;
            this.lblProcessVersion.Text = process.ProcessVersion;

            //表单相关
            SysForm form = this.DataHelper.FindById<SysForm>(process.FormId);
            if (form != null)
            {
                this.lblFormName.Text = form.FormName;

                SysEntity entity = GetEntity(form.EntityId);
                if (entity != null)
                {
                    this.lblProcessEntity.Text = entity.DisplayText;
                }
                else
                {
                    AddError(new Exception("流程实体为空，请至表单配置页面设置"));
                }
            }
            else
            {
                AddError(new Exception("流程关联表单为空，请返回至第一步配置"));
            }

            //流程参与人
            var ppCount = this.DataHelper.Set<SysProcessParticipant>()
                .Where(p => p.ProcessId == this.ProcessId).Count();
            if (ppCount == 0)
            {
                AddError(new Exception("流程参与人未配置，请返回至第一步配置"));
            }

            //活动相关
            var activityList = this.DataHelper.Set<SysActivity>()
                        .Where(p => p.ProcessId == this.ProcessId).OrderBy(p=>p.DisplayOrder??0).ToList();

            this.gcActivity.DataSource = activityList.Select(p => new
            {
                p.ActivityName,
                ActivityType = EnumHelper.GetDescription((ActivityType)p.ActivityType),
                ExecuteType = EnumHelper.GetDescription((ActivityExecType)p.ExecType),
                ActivityParticipant = GetActivityParticipant(p),
            }).ToList();
            this.gcActivity.DataBind();

            if (activityList.Count(p => p.ActivityType == (int)ActivityType.Start) != 1)
            {
                AddError(new Exception("流程开始活动内部错误，请重建流程"));
                return;
            }
            if (activityList.Count(p => p.ActivityType == (int)ActivityType.End) != 2)
            {
                AddError(new Exception("流程结束活动内部错误，请重建流程"));
                return;
            }
            if (activityList.Count(p => p.ActivityType == (int)ActivityType.Approve) == 0)
            {
                AddError(new Exception("流程审核活动内部错误，请重建流程"));
                return;
            }

            foreach (var a in activityList)
            {
                if (string.IsNullOrEmpty(a.ActivityName))
                {
                    AddError(new Exception("活动名称为空，请返回至第二步配置"));
                }
                if (a.ActivityType == (int)ActivityType.Approve)
                {
                    var apCount = this.DataHelper.Set<SysActivityParticipant>()
                    .Where(p => p.ActivityId == a.ActivityId).Count();
                    if (apCount == 0)
                    {
                        AddError(new Exception(string.Format("活动【{0}】参与人未配置，请返回至第二步配置", a.ActivityName)));
                    }
                }
            }

            //连接线完整性验证
            List<SysTransition> tList = this.DataHelper.Set<SysTransition>()
                .Where(p => p.ProcessId == this.ProcessId).ToList();
            Queue<SysActivity> queue = new Queue<SysActivity>();
            SysActivity start = activityList.FirstOrDefault(p => p.ActivityType == (int)ActivityType.Start);
            SysActivity temp = start;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                temp = queue.Dequeue();
                SysTransition t = tList.FirstOrDefault(p => p.PreActivityId == temp.ActivityId
                    && p.Direction == (int)FlowStepDirection.True);
                if (t != null)
                {
                    temp = activityList.FirstOrDefault(p => p.ActivityId == t.PostActivityId);
                    queue.Enqueue(temp);
                }
                else
                {
                    break;
                }
            }
            if (!(temp.ActivityType == (int)ActivityType.End && temp.ActivityName == "通过"))
            {
                AddError(new Exception(string.Format("流程在活动【{0}】处断开，内部错误，请重建流程", temp.ActivityName)));
            }
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormProcessDesigner_ThirdStep.aspx?id={0}", this.ProcessId));
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                SysProcess process = this.DataHelper.FindById<SysProcess>(this.ProcessId);
                FormProcessPublishHelper helper = new FormProcessPublishHelper(process);
                helper.Publish();

                this.AjaxAlertAndRedirect("发布成功", "FormProcessQuery.aspx");             
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }
    }

    public class FormProcessPublishHelper
    {        
        private SysProcess _process;

        public FormProcessPublishHelper(SysProcess process)
        {            
            this._process = process;
        }

        /// <summary>
        /// 启用流程，只有“停用”状态的可以启用
        /// </summary>
        public void StartProcess()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (BizDataContext db = new BizDataContext())
                {
                    if (_process != null && _process.ProcessStatus == (int)ProcessState.Stoped)
                    {
                        int count = db.Set<SysProcess>().Count(p => p.ProcessType == _process.ProcessType
                            && p.ProcessStatus == (int)ProcessState.StartUsed);
                        if (count > 0)
                        {
                            throw new Exception("该流程已经存在已启用的版本，请先停用");
                        }                        

                        _process.ProcessStatus = (int)ProcessState.StartUsed;
                        db.UpdatePartial(_process, p => new { p.ProcessStatus });
                    }
                    else
                    {
                        throw new Exception("流程不存在或当前流程状态不能启用");
                    }
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 停用流程，只有“启用”状态的可以停用
        /// </summary>
        public void StopProcess()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (BizDataContext db = new BizDataContext())
                {
                    if (_process != null && _process.ProcessStatus == (int)ProcessState.StartUsed)
                    {
                        _process.ProcessStatus = (int)ProcessState.Stoped;
                        db.UpdatePartial(_process, p => new { p.ProcessStatus });
                    }
                    else
                    {
                        throw new Exception("流程不存在或当前流程状态不能停用");
                    }
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 删除流程，只有“新增”状态的可以删除
        /// </summary>
        public void DeleteProcess()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (BizDataContext db = new BizDataContext())
                {
                    if (_process != null && _process.ProcessStatus == (int)ProcessState.Created)
                    {
                        //删除流程
                        db.Delete(_process);

                        //删除活动
                        db.Where<SysActivity>(p => p.ProcessId == _process.ProcessId).ToList().ForEach(p =>
                        {
                            //删除活动
                            db.Delete(p);

                            //删除活动参与人
                            db.Where<SysActivityParticipant>(i => i.ActivityId == p.ActivityId).ToList().ForEach(i => db.Delete(i));
                        });

                        //删除流程参与人
                        db.Where<SysProcessParticipant>(p => p.ProcessId == _process.ProcessId).ToList().ForEach(p => db.Delete(p));

                        //删除连接线
                        db.Where<SysTransition>(p => p.ProcessId == _process.ProcessId).ToList().ForEach(p => db.Delete(p));
                    }
                    else
                    {
                        throw new Exception("流程不存在或当前流程状态不能删除");
                    }
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 复制流程，从“已启用”或“已升级”的流程进入设计，需要先复制一份流程供设计
        /// 状态为“新增”，版本为当前最高版本 + 1
        /// </summary>
        public SysProcess CopyProcess()
        {
            SysProcess result;
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (BizDataContext db = new BizDataContext())
                {
                    //新流程
                    SysProcess newP = _process.ConvertTo<SysProcess>();
                    newP.ProcessId = db.GetNextIdentity();

                    //版本处理
                    newP.ProcessStatus = (int)ProcessState.Created;
                    string maxVersion = db.Set<SysProcess>().Where(p => p.ProcessType == _process.ProcessType).OrderByDescending(p => p.ProcessVersion).First().ProcessVersion;
                    newP.ProcessVersion = (maxVersion.ToInt() + 1).ToString();

                    result = newP;
                    db.Insert(newP);

                    //新流程参与人
                    var ppList = db.Set<SysProcessParticipant>().Where(p => p.ProcessId == _process.ProcessId).ToList();
                    Dictionary<long?, SysProcessParticipant> tempPpDict = new Dictionary<long?, SysProcessParticipant>();                    
                    foreach (var pp in ppList)
                    {  
                        SysProcessParticipant newPp = pp.ConvertTo<SysProcessParticipant>();
                        newPp.ParticipantId = db.GetNextIdentity();
                        newPp.ProcessId = newP.ProcessId;

                        tempPpDict[pp.ParticipantId] = newPp; //记录原始ID和新对象的关系                       
                    }
                    foreach (var newPp in tempPpDict.Values)
                    {
                        if (newPp.Param_ParticipantId != null) //通过刚才记录的关系找到实际的值
                        {
                            newPp.Param_ParticipantId = tempPpDict[newPp.Param_ParticipantId].ParticipantId;
                        }
                        db.Insert(newPp);
                    }

                    //新活动
                    var aList = db.Set<SysActivity>().Where(p => p.ProcessId == _process.ProcessId).ToList();
                    Dictionary<long?, SysActivity> tempADict = new Dictionary<long?, SysActivity>();
                    foreach (var a in aList)
                    {
                        SysActivity newA = a.ConvertTo<SysActivity>();
                        newA.ActivityId = db.GetNextIdentity();
                        newA.ProcessId = newP.ProcessId;

                        tempADict[a.ActivityId] = newA;//记录原始ID和新对象的关系  
                        db.Insert(newA);

                        //新活动参与人
                        var apList = db.Set<SysActivityParticipant>().Where(p => p.ActivityId == a.ActivityId).ToList();
                        foreach (var ap in apList)
                        {
                            SysActivityParticipant newAp = ap.ConvertTo<SysActivityParticipant>();
                            newAp.ActivityParticipantId = db.GetNextIdentity();
                            newAp.ActivityId = newA.ActivityId;
                            newAp.ParticipantId = tempPpDict[newAp.ParticipantId].ParticipantId; //通过刚才记录的关系找到实际的值
                            db.Insert(newAp);
                        }
                    }

                    //连接线
                    var tList = db.Set<SysTransition>().Where(p => p.ProcessId == _process.ProcessId).ToList();
                    foreach (var t in tList)
                    {
                        SysTransition newT = t.ConvertTo<SysTransition>();
                        newT.TransitionId = db.GetNextIdentity();
                        newT.ProcessId = newP.ProcessId;

                        //通过刚才记录的关系找到实际的值
                        newT.PreActivityId = tempADict[newT.PreActivityId].ActivityId;
                        newT.PostActivityId = tempADict[newT.PostActivityId].ActivityId;

                        db.Insert(newT);
                    }

                    //表单活动权限
                    var fpList = db.Set<SysFormPrivilege>().Where(p => p.ProcessId == _process.ProcessId).ToList();
                    foreach (var fp in fpList)
                    {
                        SysFormPrivilege newFp = fp.ConvertTo<SysFormPrivilege>();
                        newFp.PrivilegeId = db.GetNextIdentity();
                        newFp.ProcessId = newP.ProcessId;
                        newFp.ActivityId = tempADict[newFp.ActivityId].ActivityId;//通过刚才记录的关系找到实际的值

                        db.Insert(newFp);
                    }
                }
                ts.Complete();
            }
            return result;
        }

        /// <summary>
        /// 发布
        /// </summary>
        public void Publish()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (BizDataContext db = new BizDataContext())
                {
                    //TO DO 验证流程的完整性

                    //将对应的已启用的流程的状态改为已升级
                    SysProcess oldProcess = db.FirstOrDefault<SysProcess>(p => p.ProcessType == _process.ProcessType
                        && p.ProcessStatus == (int)ProcessState.StartUsed);
                    if (oldProcess != null)
                    {
                        oldProcess.ProcessStatus = (int)ProcessState.Updated;
                        db.UpdatePartial(oldProcess, p => new { p.ProcessStatus });
                    }

                    //将当前流程版本，状态改为已启用                    
                    _process.ProcessStatus = (int)ProcessState.StartUsed;
                    db.UpdatePartial(_process, p => new { p.ProcessStatus });                    
                }
                ts.Complete();
            }
        }
    }
}