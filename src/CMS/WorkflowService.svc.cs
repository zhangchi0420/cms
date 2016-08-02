//作者：朱旻
//日期：2011-11-8
//说明：工作流设计器所调用的WCF服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Drision.Framework.Common;
using Drision.Framework.Common.ServiceContract;
using Drision.Framework.OrgLibrary;
using Drision.Framework.Interfaces;
using System.Threading;
using Drision.Framework.Common.Workflow;
using Drision.Framework.WorkflowEngineCore.Cache;
using Drision.Framework.WorkflowEngineCore;


namespace Drision.Framework.MetaData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“WorkflowService”。
    public class WorkflowService : IWorkflowService
    {
        //回调接口，用于向客户端发消息
        private IWorkflowServiceCallBack callBack; 

        public WorkflowService()
        {
            if (OperationContext.Current != null)
            {
                callBack = OperationContext.Current.GetCallbackChannel<IWorkflowServiceCallBack>();
            }
        }

        /// <summary>
        /// 查询所有实体（含所有字段、引用实体、引用实体的字段）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfEntity> GetEntityList()
        {
            Drision.Framework.Entity.EntityCache cache = Drision.Framework.Entity.EntityCache.New();
            //using (BizDataContext context = new BizDataContext())
            {
                //var entityList = context.Set<SysEntity>().OrderBy(p => p.DisplayText).ToList();
                var entityList = cache.SysEntity.OrderBy(p => p.DisplayText).ToList();
                //var fieldList = context.Set<SysField>().OrderBy(p => p.DisplayText).ToList();
                var fieldList = cache.SysField.OrderBy(p => p.DisplayText).ToList();
                //var relationList = context.FetchAll<SysOneMoreRelation>();
                var relationList = cache.SysOneMoreRelation;
                //var pageList = context.Where<SysPage>(p => p.PageType == (int)PageType.Activity || p.PageType == (int)PageType.Approve);
                var pageList = cache.SysPage.Where(p => p.PageType == (int)PageType.Activity || p.PageType == (int)PageType.Approve);

                //foreach (var page in pageList)
                //{
                //    var control = context.FindById<SysPageControl>(page.ControlId);
                //    page.EntityId = control.EntityId;
                //}

                //foreach (var entity in entityList)
                //{
                //    entity.ChildOneMoreRelations = relationList.Where(p => p.ChildEntityId == entity.EntityId).ToList();
                //    foreach (var relation in entity.ChildOneMoreRelations)
                //    {
                //        relation.ChildField = fieldList.FirstOrDefault(p => p.FieldId == relation.ChildFieldId);
                //        relation.ParentEntity = entityList.FirstOrDefault(p => p.EntityId == relation.ParentEntityId);
                //    }

                //    entity.Fields = fieldList.Where(p => p.EntityId == entity.EntityId).ToList();
                //}

                var result = entityList.Select(p => new WfEntity()
                {
                    EntityId = p.EntityId,
                    EntityName = p.DisplayText,
                    RelationList = p.ChildOneMoreRelations.Select(r => new WfRelation() //所有引用实体（关系）
                    {
                        RelationId = r.RelationId,
                        RelationName = r.ChildField.DisplayText,
                        ParentEntityId = r.ParentEntity.EntityId,
                        ParentEntityName = r.ParentEntity.DisplayText,
                    }).ToList(),
                    FieldList = p.Fields.Select(f => new WfField() //所有字段
                    {
                        FieldId = f.FieldId,
                        DataType = f.DataType,
                        FieldName = f.DisplayText,
                    }).ToList(),
                    PageList = pageList.Where(i => i.EntityId == p.EntityId).Select(i => new WfPage()
                    {
                        PageId = i.ControlId,
                        PageName = i.DisplayText,
                    }).ToList(),
                }).ToList();

                return result;
            }
        }

        /// <summary>
        /// 查询所有提醒模板
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfRemindTemplate> GetRemindTemplateList()
        {
            using (BizDataContext context = new BizDataContext())
            {
                return context.FetchAll<SysProcessRemindTemplate>().ConvertTo<WfRemindTemplate>();
            }
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfUser> GetUserList()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var proxy = OrgProxyFactory.GetProxy(context);
                return proxy.GetUsers().OrderBy(p => p.User_Name)
                    .Select(p => new WfUser() { UserId = p.User_ID, UserName = p.User_Name });
            }
        }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfRole> GetRoleList()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var proxy = OrgProxyFactory.GetProxy(context);
                return proxy.GetRoles().OrderBy(p => p.Role_Name)
                    .Select(p => new WfRole() { RoleId = p.Role_ID, RoleName = p.Role_Name });
            }
        }

        /// <summary>
        /// 查询所有部门
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfDepartment> GetDepartmentList()
        {
            using (BizDataContext context = new BizDataContext())
            {
                var proxy = OrgProxyFactory.GetProxy(context);
                return proxy.GetDepartments().OrderBy(p => p.Department_Name)
                    .Select(p => new WfDepartment() { DepartmentId = p.Department_ID, DepartmentName = p.Department_Name });
            }
        }

        /// <summary>
        /// 给当前的自动ID加上count，然后获得当前的自动ID
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public long GetCurrentIdentity(long count)
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    long result = 0;
                    for (int i = 0; i < count; i++)
                    {
                        result = context.GetNextIdentity();
                    }
                    return result - count + 1;
                }
            }
            catch
            {                
                return -1;
            }
        }

        /// <summary>
        /// 根据ID查询工作流
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public WfWorkflowData GetWorkFlow(long processId)
        {
            using (BizDataContext context = new BizDataContext())
            {
                var pCache = new ProcessCacheFactory(context);
                WfWorkflowData data = new WfWorkflowData();
                SysProcess process = pCache.GetProcessCache(processId);
                if (process != null)
                {
                    //流程定义
                    data.Process = process.ConvertTo<WfProcess>();
                    if (data.Process.TemplateId != null)
                    {
                        data.Process.ProcessTemplate = Convert.ToBase64String(data.Process.TemplateId);
                        data.Process.TemplateId = null;
                    }

                    //2013-9-11 zhumin 这里的版本号直接返回当前最高版本 + 1，否则从“已升级”的载入，会版本错误
                    //载入“新增”状态的流程，版本不变		
                    //载入其它状态的流程，复制一份为“新增”，版本为当前最高+1		
                    //发布流程，版本不变
                    //新增第一个流程，版本为1
                    if (data.Process.ProcessStatus != (int)ProcessState.Created)
                    {
                        string maxVersion = context.Set<SysProcess>().Where(p => p.ProcessType == process.ProcessType).OrderByDescending(p => p.ProcessVersion).First().ProcessVersion;                    
                        data.Process.ProcessVersion = (maxVersion.ToInt() + 1).ToString();
                    }
                    

                    //流程提醒 
                    data.ProcessRemindList = process.ProcessReminds.ConvertTo<WfProcessRemind>();
                    var prpList = new List<SysProcessRemindParticipant>();
                    foreach (var pr in process.ProcessReminds)
                    {
                        prpList = prpList.Union(pr.RemindParticipants).ToList();
                    }
                    data.ProcessRemindParticipantList = prpList.ConvertTo<WfProcessRemindParticipant>();

                    //活动
                    data.ActivityList = process.Activities.ConvertTo<WfActivity>();

                    //活动参与人和活动提醒
                    var arList = new List<SysActivityRemind>();
                    var apList = new List<SysActivityParticipant>();
                    var arpList = new List<SysActivityRemindParticipant>();
                    var aoList = new List<SysActivityOperation>();
                    var aosList = new List<SysActivityStep>();
                    foreach (var a in process.Activities)
                    {
                        arList = arList.Union(a.ActivityReminds).ToList();
                        apList = apList.Union(a.ActivityParticipants).ToList();
                        aoList = aoList.Union(a.ActivityOperations).ToList();

                        foreach (var ar in a.ActivityReminds)
                        {
                            arpList = arpList.Union(ar.RemindParticipants).ToList();
                        }

                        foreach (var ao in a.ActivityOperations)
                        {
                            aosList = aosList.Union(ao.ActivitySteps).ToList();
                        }
                    }
                    data.ActivityRemindList = arList.ConvertTo<WfActivityRemind>();
                    data.ActivityParticipantList = apList.ConvertTo<WfActivityParticipant>();
                    data.ActivityRemindParticipantList = arpList.ConvertTo<WfActivityRemindParticipant>();

                    //活动操作
                    data.ActivityOperationList = aoList.ConvertTo<WfActivityOperation>();
                    data.ActivityStepList = aosList.ConvertTo<WfActivityStep>();

                    //条件、参与人、连接线
                    data.ExpressionList = process.Expressions.ConvertTo<WfExpression>();
                    data.ParticipantList = process.ProcessParticipants.ConvertTo<WfProcessParticipant>();
                    data.TransitionList = process.Transitions.ConvertTo<WfTransition>();
                }

                pCache.ClearCache(processId);

                return data;
            }
        }

        /// <summary>
        /// 查询所有工作流
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WfProcess> GetWorkFlowList()
        {
            Drision.Framework.Entity.EntityCache cache = Drision.Framework.Entity.EntityCache.New();
            using (BizDataContext context = new BizDataContext())
            {
                var processList = context.Set<SysProcess>().Where(p => p.ProcessCategory != (int)ProcessCategory.FormApprove).ToList();
                foreach (var p in processList)
                {
                    p.ActivityEntity = cache.FindById<SysEntity>(p.ActivityEntityId);// context.FindById<SysEntity>(p.ActivityEntityId);
                    p.ProcessEntity = cache.FindById<SysEntity>(p.EntityId);// context.FindById<SysEntity>(p.EntityId);
                }

                return processList.Select(p => new
                {
                    ActivityEntityName = p.ActivityEntity != null ? p.ActivityEntity.EntityName : "",
                    EntityName = p.ProcessEntity != null ? p.ProcessEntity.EntityName : "",
                    p.ProcessName,
                    p.ProcessId,
                    p.ProcessStatus,
                    p.ProcessCategory,
                    p.ProcessVersion,
                    CanDelete = p.ProcessStatus == (int)ProcessState.Created,
                }).ConvertTo<WfProcess>();
            }
        }
               
        /// <summary>
        /// 发布工作流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string PublishWorkFlow(WfWorkflowData data, long? oldProcessId)
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    //先把对应的已启用的流程状态改为已升级
                    var oldProcess = context.FirstOrDefault<SysProcess>(p => p.ProcessType == data.Process.ProcessType && p.ProcessStatus == (int)ProcessState.StartUsed);
                    if (oldProcess != null)
                    {
                        oldProcess.ProcessStatus = (int)ProcessState.Updated;
                        context.UpdatePartial(oldProcess, p => new { p.ProcessStatus });
                    }

                    return InternalSaveWorkFlow(context,data, oldProcessId);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string InternalSaveWorkFlow(BizDataContext context,WfWorkflowData data, long? oldProcessId)
        {
            try
            {
                SendToClient("正在禁用外键约束...", 5);
                DisableConstraint();

                if (oldProcessId != null)
                {
                    Delete(oldProcessId.Value);
                }

                SendToClient("正在保存流程...", 20);
                SysProcess process = data.Process.ConvertTo<SysProcess>();
                if (!string.IsNullOrEmpty(data.Process.ProcessTemplate))
                {
                    process.TemplateId = Convert.FromBase64String(data.Process.ProcessTemplate);
                }
                context.Insert(process);

                SendToClient("正在保存活动列表...", 30);
                data.ActivityList.ConvertTo<SysActivity>().ForEach(p => context.Insert(p));

                SendToClient("正在保存活动自动操作...", 35);
                data.ActivityOperationList.ConvertTo<SysActivityOperation>().ForEach(p => context.Insert(p));

                SendToClient("正在保存活动自动操作步骤...", 40);
                data.ActivityStepList.ConvertTo<SysActivityStep>().ForEach(p => context.Insert(p));

                SendToClient("正在保存活动提醒...", 45);
                data.ActivityRemindList.ConvertTo<SysActivityRemind>().ForEach(p => context.Insert(p));

                SendToClient("正在保存活动提醒参与人...", 50);
                data.ActivityRemindParticipantList.ConvertTo<SysActivityRemindParticipant>().ForEach(p => context.Insert(p));

                SendToClient("正在保存活动参与人...", 60);
                data.ActivityParticipantList.ConvertTo<SysActivityParticipant>().ForEach(p => context.Insert(p));

                SendToClient("正在保存条件表达式列表...", 65);
                data.ExpressionList.ConvertTo<SysExpression>().ForEach(p => context.Insert(p));

                SendToClient("正在保存流程参与人列表...", 70);
                data.ParticipantList.ConvertTo<SysProcessParticipant>().ForEach(p => context.Insert(p));

                SendToClient("正在保存流程提醒列表...", 75);
                data.ProcessRemindList.ConvertTo<SysProcessRemind>().ForEach(p => context.Insert(p));

                SendToClient("正在保存流程提醒参与人列表...", 80);
                data.ProcessRemindParticipantList.ConvertTo<SysProcessRemindParticipant>().ForEach(p => context.Insert(p));

                SendToClient("正在保存连接线列表...", 90);
                data.TransitionList.ConvertTo<SysTransition>().ForEach(p => context.Insert(p));

                SendToClient("正在启用外键约束...", 95);
                EnableConstraint();

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 保存工作流
        /// </summary>
        /// <param name="process"></param>
        public string SaveWorkFlow(WfWorkflowData data, long? oldProcessId)
        {
            using (BizDataContext context = new BizDataContext())
            {
                return InternalSaveWorkFlow(context, data, oldProcessId);
            }
        }        

        /// <summary>
        /// 清除流程
        /// </summary>
        /// <param name="processId"></param>
        public string Delete(long processId)
        {
            try
            {
                using (BizDataContext context = new BizDataContext())
                {
                    SendToClient("正在删除旧流程定义...", 10);

                    SysProcess process = context.FindById<SysProcess>(processId);
                    if (process != null)
                    {
                        //禁用外键
                        DisableConstraint();

                        //删除流程
                        context.Delete(process);

                        //删除活动
                        context.Where<SysActivity>(p => p.ProcessId == processId).ToList().ForEach(p =>
                        {
                            //删除活动
                            context.Delete(p);

                            //删除活动操作
                            context.Where<SysActivityOperation>(i => i.ActivityId == p.ActivityId).ToList().ForEach(i =>
                            {
                                //删除活动操作
                                context.Delete(i);

                                //删除活动操作步骤
                                context.Where<SysActivityStep>(q => q.OperationId == i.OperationId).ToList().ForEach(q => context.Delete(q));
                            });

                            //删除活动提醒
                            context.Where<SysActivityRemind>(i => i.ActivityId == p.ActivityId).ToList().ForEach(i =>
                            {
                                //删除活动提醒
                                context.Delete(i);

                                //删除活动提醒参与人
                                context.Where<SysActivityRemindParticipant>(q => q.RemindId == i.RemindId).ToList().ForEach(q => context.Delete(q));
                            });

                            //删除活动参与人
                            context.Where<SysActivityParticipant>(i => i.ActivityId == p.ActivityId).ToList().ForEach(i => context.Delete(i));

                            //删除表达式
                            context.Where<SysExpression>(i => i.ActivityId == p.ActivityId).ToList().ForEach(i => context.Delete(i));
                        });

                        //删除流程参与人
                        context.Where<SysProcessParticipant>(p => p.ProcessId == processId).ToList().ForEach(p => context.Delete(p));

                        //删除流程提醒
                        context.Where<SysProcessRemind>(p => p.ProcessId == processId).ToList().ForEach(p =>
                        {
                            //删除流程提醒
                            context.Delete(p);

                            //删除流程提醒参与人
                            context.Where<SysProcessRemindParticipant>(q => q.RemindId == p.RemindId).ToList().ForEach(q => context.Delete(q));
                        });

                        //删除连接线
                        context.Where<SysTransition>(p => p.ProcessId == processId).ToList().ForEach(p => context.Delete(p));

                        //启用外键
                        EnableConstraint();

                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }        

        /// <summary>
        /// 禁用外键约束
        /// </summary>
        private void DisableConstraint()
        {            
            //string strSQL = "exec sp_msforeachtable \"alter table ? nocheck CONSTRAINT all\"";
            //context.Database.ExecuteSqlCommand(strSQL); 
        }

        /// <summary>
        /// 启用外键约束
        /// </summary>
        /// <param name="percent"></param>
        private void EnableConstraint()
        {            
            //string strSQL = "exec sp_msforeachtable \"alter table ? check CONSTRAINT all\"";
            //context.Database.ExecuteSqlCommand(strSQL); 
        }

        /// <summary>
        /// 发消息到客户端
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="percent"></param>
        private void SendToClient(string msg,double percent)
        {
            if (callBack != null)
            {                
                callBack.Progress(new WfMessage()
                {
                    Message = msg,
                    Percent = percent,
                });
            }
        }
    }
}
