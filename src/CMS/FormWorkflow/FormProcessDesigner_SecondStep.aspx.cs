using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Drision.Framework.Web.Common;
using Drision.Framework.Common.Workflow;
using Drision.Framework.Common;
using System.Web.Script.Serialization;

namespace Drision.Framework.Web.FormWorkflow
{
    public partial class FormProcessDesigner_SecondStep : BasePage
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

        /// <summary>
        /// 当前选中的活动ID        
        /// </summary>
        protected long SelectedActivityId
        {
            get { return this.hc.Value.ToLong(); }
        }

        /// <summary>
        /// 当前选中的活动
        /// </summary>
        protected SysActivity SelectedActivity
        {
            get;
            set;
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
                else
                {
                    //保存活动参与人后、选择某活动后由前台手动回发
                    if (Request["__EVENTARGUMENT"] == "select")
                    {
                        SelectActivity(this.hc.Value.ToLong());
                    }
                    else if (Request["__EVENTARGUMENT"] == "partSave")
                    {
                        BindActivityParticipant();
                    }
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndRedirect(ex, "FormProcessQuery.aspx");
            }
        }

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
                this.lblProcessName.Text = process.ProcessName;
                SysEntity processEntity = GetEntity(process.EntityId);
                if (processEntity == null)
                {
                    throw new Exception("流程实体为空");
                }
                this.lblProcessEntity.Text = processEntity.DisplayText;

                //hc.Text用于回发
                //hc.Value记录当前选中的活动，由前台选中某活动，或调用后台SelectActivity方法更新
                this.hc.Text = this.ClientScript.GetPostBackEventReference(this.hc, "select");
                this.hcSave.Text = this.ClientScript.GetPostBackEventReference(this.hc, "partSave");


                AddHiddenAndFirstActivity();
                BindDropDown();
            }
            else
            {
                throw new Exception("参数不正确");
            }
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        private void BindDropDown()
        {
            //流程参与人
            var source = this.DataHelper.Where<SysProcessParticipant>(p => p.ProcessId == this.ProcessId
                && p.FunctionType != (int)ParticipantFunctionType.StartUser);
            this.ccParticipant.DataSource = source;
            this.ccParticipant.DataBind();

            //通过方式
            this.ccPassType.DataSource = typeof(ActivityPassType);
            this.ccPassType.DataBind();
        }

        /// <summary>
        /// 进入页面，如果没有任何活动，自动添加隐藏的开始活动、结束活动、以及第一个审核活动，并选中第一个审核活动
        /// </summary>
        private void AddHiddenAndFirstActivity()
        {
            int count = this.DataHelper.Set<SysActivity>()
                .Where(p => p.ProcessId == this.ProcessId).Count();
            if (count == 0)
            {
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //开始活动
                        SysActivity start = new SysActivity()
                        {
                            ActivityId = db.GetNextIdentity(),
                            ActivityName = "开始",
                            ActivityType = (int)ActivityType.Start,
                            ProcessId = this.ProcessId,
                            ExecType = (int)ActivityExecType.Auto,
                            LeftPos = 350,
                            TopPos = 50,
                        };
                        db.Insert(start);

                        //第一个审核活动
                        SysActivity first = new SysActivity()
                        {
                            ActivityId = this.DataHelper.GetNextIdentity(),
                            ProcessId = this.ProcessId,
                            ActivityType = (int)ActivityType.Approve,
                            ActivityName = "活动名称",
                            DisplayOrder = 1,
                            ExecType = (int)ActivityExecType.Manual,
                            PassType = (int)ActivityPassType.FirstDefault,
                            LeftPos = 350,
                            TopPos = 180,
                        };
                        db.Insert(first);

                        //结束活动（通过）
                        SysActivity endPass = new SysActivity()
                        {
                            ActivityId = db.GetNextIdentity(),
                            ActivityName = "通过",
                            ActivityType = (int)ActivityType.End,
                            ProcessId = this.ProcessId,
                            ExecType = (int)ActivityExecType.Auto,
                            IsPassedWithNoParticipants = true,
                            LeftPos = 100,
                            TopPos = 350,
                            ApproveResult = (int)ApproveResultEnum.Pass,
                        };
                        db.Insert(endPass);

                        //结束活动（驳回）
                        SysActivity endReject = new SysActivity()
                        {
                            ActivityId = db.GetNextIdentity(),
                            ActivityName = "驳回",
                            ActivityType = (int)ActivityType.End,
                            ProcessId = this.ProcessId,
                            ExecType = (int)ActivityExecType.Auto,
                            LeftPos = 600,
                            TopPos = 350,
                            ApproveResult = (int)ApproveResultEnum.Reject,
                        };
                        db.Insert(endReject);

                        //按默认规则链接这三个活动
                        ConnectActivity(db, start, first, FlowStepDirection.True);
                        ConnectActivity(db, first, endPass, FlowStepDirection.True);
                        ConnectActivity(db, first, endReject, FlowStepDirection.False);
                    }
                    ts.Complete();
                }
            }

            //选中第一个活动
            var a = this.DataHelper.Set<SysActivity>()
                    .Where(p => p.ProcessId == this.ProcessId && p.ActivityType == (int)ActivityType.Approve)
                    .OrderBy(p => p.DisplayOrder).FirstOrDefault();
            SelectActivity(a);
        }

        #region 活动相关

        /// <summary>
        /// 连接两个活动
        /// </summary>        
        private void ConnectActivity(BizDataContext db, SysActivity prev, SysActivity post, FlowStepDirection direction)
        {
            SysTransition t = new SysTransition()
            {
                TransitionId = db.GetNextIdentity(),
                PreActivityId = prev.ActivityId,
                PostActivityId = post.ActivityId,
                ProcessId = this.ProcessId,
                Direction = (int)direction,
            };
            switch (direction)
            {
                case FlowStepDirection.False: t.DisplayText = "驳回"; break;
                case FlowStepDirection.True: t.DisplayText = "通过"; break;
                default: break;
            }
            db.Insert(t);
        }

        /// <summary>
        /// 绑定活动列表（只显示审核活动）
        /// </summary>
        private void BindActivityList()
        {
            var source = from a in this.DataHelper.Set<SysActivity>()
                         where a.ProcessId == this.ProcessId
                         && a.ActivityType == (int)ActivityType.Approve
                         orderby a.DisplayOrder
                         select new
                         {
                             a.DisplayOrder,
                             a.ActivityId,
                             a.ActivityName,
                             SelectClass = a.ActivityId == this.SelectedActivityId ? "selected" : "unselected",
                         };

            this.rActivity.DataSource = source.ToList();
            this.rActivity.DataBind();

            //驳回方式
            this.ccRejectType.DataSource = typeof(FormProcessActivityRejectType);
            this.ccRejectType.DataBind();

            //驳回目标活动（只能选当前活动上面的的审核活动）
            this.ccRejectTargetActivity.DataSource = source.Where(p => p.DisplayOrder < this.SelectedActivity.DisplayOrder).ToList();
            this.ccRejectTargetActivity.DataBind();

            //找到当前活动驳回指向的活动
            this.divRejectTargetActivity.Visible = false;
            SysTransition t = this.DataHelper.Set<SysTransition>()
                .Where(p => p.PreActivityId == this.SelectedActivityId && p.Direction == (int)FlowStepDirection.False).FirstOrDefault();
            if (t == null)
            {
                this.ccRejectType.SelectedValue = ((int)FormProcessActivityRejectType.SelectActivityWhenRunning).ToString();
            }
            else
            {
                SysActivity target = this.DataHelper.FindById<SysActivity>(t.PostActivityId);
                if (target.ActivityType == (int)ActivityType.End)
                {
                    this.ccRejectType.SelectedValue = ((int)FormProcessActivityRejectType.EndActivity).ToString();
                }
                else
                {
                    this.ccRejectType.SelectedValue = ((int)FormProcessActivityRejectType.SelectActivity).ToString();
                    this.ccRejectTargetActivity.SelectedValue = target.ActivityId.ToString();
                    this.divRejectTargetActivity.Visible = true;
                }
            }
        }

        /// <summary>
        /// 选中某个活动
        /// </summary>
        /// <param name="activity"></param>
        private void SelectActivity(SysActivity activity)
        {
            if (activity == null)
            {
                throw new Exception("选择的活动不存在");
            }

            this.SelectedActivity = activity;
            this.hc.Value = activity.ActivityId.ToString();

            this.txtActivityName.Text = activity.ActivityName;

            //绑定活动参与人列表
            BindActivityParticipant();

            //重新绑定活动列表（更新选中状态）
            BindActivityList();
        }

        /// <summary>
        /// 选中某个活动
        /// </summary>
        /// <param name="activityId"></param>
        private void SelectActivity(long activityId)
        {
            SysActivity activity = this.DataHelper.FindById<SysActivity>(activityId);
            SelectActivity(activity);
        }

        /// <summary>
        /// 驳回方式改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ccRejectType_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FormProcessActivityRejectType rejectType = (FormProcessActivityRejectType)this.ccRejectType.SelectedValue.ToInt();
                if (rejectType != FormProcessActivityRejectType.SelectActivity)
                {
                    this.divRejectTargetActivity.Visible = false;
                }
                else
                {
                    this.divRejectTargetActivity.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        #endregion

        #region 活动参与人相关

        /// <summary>
        /// 绑定活动参与人
        /// </summary>
        private void BindActivityParticipant()
        {
            var source = from ap in this.DataHelper.Set<SysActivityParticipant>()
                         join pp in this.DataHelper.Set<SysProcessParticipant>()
                         on ap.ParticipantId equals pp.ParticipantId
                         where ap.ActivityId == this.SelectedActivityId
                         select new
                         {
                             ap.ActivityParticipantId,
                             pp.ParticipantName,
                             PassType = EnumHelper.GetDescription((ActivityPassType)ap.PassType),
                             ap.MinPassNum,
                             ap.MinPassRatio,
                         };
            this.gcAP.DataSource = source.ToList();
            this.gcAP.DataBind();
        }

        /// <summary>
        /// 删除活动参与人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteParticipant_Click(object sender, EventArgs e)
        {
            try
            {
                long? id = (sender as LinkButton).CommandArgument.ToLongNullable();
                var ap = this.DataHelper.FindById<SysActivityParticipant>(id);
                if (ap != null)
                {
                    this.DataHelper.Delete(ap);
                    BindActivityParticipant();
                }
                else
                {
                    throw new Exception("找不到活动参与人");
                }
            }
            catch (Exception ex)
            {
                this.AjaxAlert(ex);
            }
        }

        /// <summary>
        /// 保存活动参与人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbc_CallBack(object sender, CallBackEventArgs e)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string errorMsg = string.Empty; //错误消息
            bool isError = false;

            try
            {
                long? aid = e.Context["ActivityId"].ToStringNullable().ToLongNullable();
                long? pid = e.Context["ParticipantId"].ToStringNullable().ToLongNullable();
                int? passType = e.Context["PassType"].ToStringNullable().ToIntNullable();
                int? minNum = e.Context["MinPassNum"].ToStringNullable().ToIntNullable();
                decimal? minRatio = e.Context["MinPassRatio"].ToStringNullable().ToDecimalNullable();

                if (aid == null)
                {
                    throw new Exception("当前活动为空");
                }
                if (pid == null)
                {
                    throw new Exception("流程参与人不能为空");
                }
                if (passType == null)
                {
                    throw new Exception("通过方式不能为空");
                }
                if (passType == (int)ActivityPassType.UsingMinNum && minNum == null)
                {
                    throw new Exception("最小通过数不能为空");
                }
                if (passType == (int)ActivityPassType.UsingMinRatio && minRatio == null)
                {
                    throw new Exception("最小通过率不能为空");
                }

                var count = this.DataHelper.Set<SysActivityParticipant>()
                    .Where(p => p.ParticipantId == pid && p.ActivityId == aid).Count();
                if (count > 0)
                {
                    throw new Exception("已存在相同的活动参与人");
                }

                SysActivityParticipant ap = new SysActivityParticipant()
                {
                    ActivityParticipantId = this.DataHelper.GetNextIdentity(),
                    ActivityId = aid,
                    PassType = passType,
                    MinPassNum = minNum,
                    MinPassRatio = (double?)minRatio,
                    IsMustApprove = true,
                    SumAfterAllComplete = false,
                    ParticipantId = pid,
                };
                this.DataHelper.Insert(ap);
            }
            catch (Exception ex)
            {
                isError = true;
                errorMsg = ex.Message;
            }

            string result = js.Serialize(new
            {
                IsError = isError,
                Error = errorMsg,
            });
            e.Result = result;
        }

        #endregion

        #region 按钮操作

        /// <summary>
        /// 保存当前活动配置
        /// </summary>        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //所有活动
                Dictionary<long, SysActivity> activityDict = this.DataHelper.Set<SysActivity>()
                        .Where(p => p.ProcessId == this.ProcessId).ToDictionary(p => p.ActivityId);

                //当前活动
                SysActivity a = activityDict[this.SelectedActivityId];

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        a.ActivityName = this.txtActivityName.Text.Trim();
                        db.UpdatePartial(a, p => new { p.ActivityName });

                        #region 处理驳回方式
                        //找到当前活动驳回指向的连接线
                        SysTransition t = this.DataHelper.Set<SysTransition>()
                                .Where(p => p.PreActivityId == this.SelectedActivityId && p.Direction == (int)FlowStepDirection.False).FirstOrDefault();
                        FormProcessActivityRejectType rejectType = (FormProcessActivityRejectType)this.ccRejectType.SelectedValue.ToInt();
                        switch (rejectType)
                        {
                            case FormProcessActivityRejectType.SelectActivityWhenRunning:
                                {
                                    if (t != null)
                                    {
                                        db.Delete(t);
                                    }
                                    break;
                                }
                            case FormProcessActivityRejectType.SelectActivity:
                                {
                                    SysActivity target = activityDict[this.ccRejectTargetActivity.SelectedValue.ToLong()];
                                    if (t == null)
                                    {
                                        ConnectActivity(db, a, target, FlowStepDirection.False);
                                    }
                                    else if (t.PostActivityId != target.ActivityId)
                                    {
                                        t.PostActivityId = target.ActivityId;
                                        db.UpdatePartial(t, p => new { p.PostActivityId });
                                    }
                                    break;
                                }
                            case FormProcessActivityRejectType.EndActivity:
                                {
                                    SysActivity end = activityDict.Values.FirstOrDefault(p => p.ActivityType == (int)ActivityType.End && p.ActivityName == "驳回");
                                    if (t == null)
                                    {
                                        ConnectActivity(db, a, end, FlowStepDirection.False);
                                    }
                                    else if (t.PostActivityId != end.ActivityId)
                                    {
                                        t.PostActivityId = end.ActivityId;
                                        db.UpdatePartial(t, p => new { p.PostActivityId });
                                    }
                                    break;
                                }
                            default: break;
                        }
                        #endregion
                    }
                    ts.Complete();
                }

                //重新选择一下
                SelectActivity(a);

                this.AjaxAlertAndEnableButton("保存成功");
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 新增一个活动，并选中
        /// </summary>        
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //所有活动
                Dictionary<long, SysActivity> activityDict = this.DataHelper.Set<SysActivity>()
                        .Where(p => p.ProcessId == this.ProcessId).ToDictionary(p => p.ActivityId);

                //最后一个审核活动
                SysActivity lastActivity = activityDict.Values.Where(p => p.ActivityType == (int)ActivityType.Approve)
                    .OrderByDescending(p => p.DisplayOrder).FirstOrDefault();

                SysActivity endReject = activityDict.Values.FirstOrDefault(p => p.ActivityType == (int)ActivityType.End && p.ActivityName == "驳回");
                SysActivity endPass = activityDict.Values.FirstOrDefault(p => p.ActivityType == (int)ActivityType.End && p.ActivityName == "通过");

                int maxCount = activityDict.Values.Max(p => p.DisplayOrder ?? 0);

                //最后一个审核活动通过指向的连接线
                SysTransition t = this.DataHelper.Set<SysTransition>()
                                .Where(p => p.PreActivityId == lastActivity.ActivityId
                                    && p.Direction == (int)FlowStepDirection.True).FirstOrDefault();

                SysActivity newActivity;
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //新活动
                        newActivity = new SysActivity()
                        {
                            ActivityId = db.GetNextIdentity(),
                            ProcessId = this.ProcessId,
                            ActivityType = (int)ActivityType.Approve,
                            DisplayOrder = maxCount + 1,
                            ExecType = (int)ActivityExecType.Manual,
                            PassType = (int)ActivityPassType.FirstDefault,
                            IsPassedWithNoParticipants = true,
                            ActivityName = "活动名称",
                            LeftPos = 350,
                            TopPos = (maxCount + 1) * 130 + 50,
                        };
                        db.Insert(newActivity);

                        //把上一个活动接到新活动
                        t.PostActivityId = newActivity.ActivityId;
                        db.UpdatePartial(t, p => new { p.PostActivityId });

                        //按默认规则链接新活动和结束活动
                        ConnectActivity(db, newActivity, endPass, FlowStepDirection.True);
                        ConnectActivity(db, newActivity, endReject, FlowStepDirection.False);
                    }
                    ts.Complete();
                }

                //选中新活动
                SelectActivity(newActivity);

                this.EnableButton();
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //所有活动
                Dictionary<long, SysActivity> activityDict = this.DataHelper.Set<SysActivity>()
                        .Where(p => p.ProcessId == this.ProcessId).ToDictionary(p => p.ActivityId);

                if (activityDict.Count == 4)
                {
                    throw new Exception("只有一个审核活动，不能删除");
                }

                //当前活动
                SysActivity a = activityDict[this.SelectedActivityId];

                //当前活动参与人
                List<SysActivityParticipant> apList = this.DataHelper.Set<SysActivityParticipant>()
                    .Where(p => p.ActivityId == this.SelectedActivityId).ToList();

                //此活动之后的审核活动
                List<SysActivity> nextActivityList = activityDict.Values.Where(p => (p.DisplayOrder ?? 0) > a.DisplayOrder).ToList();

                //最后一个审核活动
                SysActivity lastActivity = activityDict.Values.OrderByDescending(p => p.DisplayOrder ?? 0).FirstOrDefault();

                //结束活动（驳回）
                SysActivity endReject = activityDict.Values.FirstOrDefault(p => p.ActivityType == (int)ActivityType.End && p.ActivityName == "驳回");

                //通过指向当前活动的连接线
                SysTransition tIn_Pass = this.DataHelper.Set<SysTransition>()
                                .Where(p => p.PostActivityId == this.SelectedActivityId
                                    && p.Direction == (int)FlowStepDirection.True).FirstOrDefault();

                //驳回指向当前活动的连接线（集合）
                List<SysTransition> tIn_RejectList = this.DataHelper.Set<SysTransition>()
                    .Where(p => p.PostActivityId == this.SelectedActivityId
                        && p.Direction == (int)FlowStepDirection.False).ToList();

                //从当前活动通过指向的连接线
                SysTransition tOut_Pass = this.DataHelper.Set<SysTransition>()
                    .Where(p => p.PreActivityId == this.SelectedActivityId
                        && p.Direction == (int)FlowStepDirection.True).FirstOrDefault();

                //从当前活动驳回指向的连接线
                SysTransition tOut_Reject = this.DataHelper.Set<SysTransition>()
                    .Where(p => p.PreActivityId == this.SelectedActivityId
                        && p.Direction == (int)FlowStepDirection.False).FirstOrDefault();

                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    using (BizDataContext db = new BizDataContext())
                    {
                        //让上一个活动通过指向当前活动通过指向的活动
                        tIn_Pass.PostActivityId = tOut_Pass.PostActivityId;
                        db.UpdatePartial(tIn_Pass, p => new { p.PostActivityId });

                        //让其它驳回指向当前活动的活动驳回指向结束活动（驳回）
                        foreach (var tIn_Reject in tIn_RejectList)
                        {
                            tIn_Reject.PostActivityId = endReject.ActivityId;
                            db.UpdatePartial(tIn_Reject, p => new { p.PostActivityId });
                        }

                        //删除当前活动及连接线
                        db.Delete(a);
                        db.Delete(tOut_Pass);
                        if (tOut_Reject != null)
                        {
                            db.Delete(tOut_Reject);
                        }

                        //删除当前活动参与人
                        foreach (var ap in apList)
                        {
                            db.Delete(ap);
                        }

                        //将后面的活动的顺序提前1位
                        foreach (var na in nextActivityList)
                        {
                            na.DisplayOrder--;
                            na.TopPos -= 130;
                            db.UpdatePartial(na, p => new { p.DisplayOrder, p.TopPos });
                        }
                    }
                    ts.Complete();
                }

                //选择最后一个活动
                SelectActivity(lastActivity);

                this.EnableButton();
            }
            catch (Exception ex)
            {
                this.AjaxAlertAndEnableButton(ex);
            }
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormProcessDesigner_FirstStep.aspx?id={0}", this.ProcessId));
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FormProcessDesigner_ThirdStep.aspx?id={0}", this.ProcessId));
        }

        #endregion
    }
}