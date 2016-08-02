//作者：朱旻
//日期：2011-11-24
//说明：工作流设计器所调用的WCF服务Contract接口，用于追踪流程和活动实例的运行状态

using System;
using System.Collections.Generic;
using System.Linq;
using Drision.Framework.Common.ServiceContract;
using Drision.Framework.Common;
using Drision.Framework.Common.Workflow;

using Drision.Framework.Entity;
using Drision.Framework.Manager;
using Drision.Framework.WorkflowEngineCore.Cache;
//using Drision.Framework.Meta;

namespace Drision.Framework.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“WorkflowInstanceService”。
    public class WorkflowInstanceService : IWorkflowInstanceService
    { 
        /// <summary>
        /// 查询流程实体数据
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <returns></returns>
        public WfWorkflowInstanceData GetWorkflowInstance(int processInstanceId)
        {
            using (BizDataContext context = new BizDataContext())
            {
                var piCache = new ProcessInstanceCacheFactory(context);

                //实例信息
                SysProcessInstance pi = piCache.GetProcessInstanceCache(processInstanceId);
                SysProcess process = pi.Process;

                //定义信息
                WfWorkflowInstanceData data = new WfWorkflowInstanceData();
                data.Process = process.ConvertTo<WfProcess>();
                if (data.Process.TemplateId != null)
                {
                    data.Process.ProcessTemplate = Convert.ToBase64String(data.Process.TemplateId);
                    data.Process.TemplateId = null;
                }
                data.ActivityList = process.Activities.ConvertTo<WfActivity>();
                data.TransitionList = process.Transitions.ConvertTo<WfTransition>();

                data.ProcessInstance = pi.ConvertTo<WfProcessInstance>();
                data.ActivityInstanceList = pi.ActivityInstances.ConvertTo<WfActivityInstance>();

                //加载活动实体信息
                foreach (var ai in data.ActivityInstanceList)
                {
                    LoadActivityEntityData(context,process, ai, data);
                }

                //连接线实例，这里表中少了个字段ProcessInstanceId，所以只能用这种方式来获取            
                data.TransitionInstanceList = new List<WfTransitionInstance>();
                foreach (var ai in pi.ActivityInstances)
                {
                    var tempList = ai.FromTransitionInstances.ConvertTo<WfTransitionInstance>();
                    data.TransitionInstanceList = data.TransitionInstanceList.Union(tempList).ToList();
                }

                piCache.ClearCache(processInstanceId);

                return data;
            }
        }

        
        /// <summary>
        /// 忽略不显示的实体字段
        /// </summary>
        private static IEnumerable<string> IgnoreList = new List<string>() { 
            ConstFieldNames.CreateTime, 
            ConstFieldNames.CreateUserId, 
            ConstFieldNames.UpdateTime, 
            ConstFieldNames.UpdateUserId,
            "UpdateUser",
            "StateEnum", 
            ConstFieldNames.StateDetail,
            ConstFieldNames.OwnerId,"Owner" };

        private WfActivityEntityInstance GetApproveEntityData(BizDataContext context,SysApproveActivityData p)
        {
            WfActivityEntityInstance entity = new WfActivityEntityInstance()
            {
                ActivityEntityInstanceId = p.DataId,
            };
            var dic = new Dictionary<string, WfActivityFieldData>();
            var list = context.Where <SysApproveActivityData>(i => i.ApproveGroupId == p.ApproveGroupId).OrderBy(i => i.ApproveTime).ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                p = list.ElementAt(i);
                var user = context.FindById<T_User>(p.ApproveUserId);
                if (user != null)
                {
                    dic[(1 + 3 * i).ToString()] = new WfActivityFieldData() { DisplayText = "审核人", Value = user.User_Name };
                }
                dic[(2 + 3 * i).ToString()] = new WfActivityFieldData() { DisplayText = "审核时间", Value = Convert.ToString(p.ApproveTime) };
                dic[(3 + 3 * i).ToString()] = new WfActivityFieldData() { DisplayText = "审核意见", Value = p.ApproveComment };
            }
            entity.ActivityEntityInstanceData = dic;

            return entity;
        }

        /// <summary>
        /// 活动实体的内容
        /// </summary>
        /// <param name="process"></param>
        /// <param name="activityInstance"></param>
        /// <param name="data"></param>
        private void LoadActivityEntityData(BizDataContext context,SysProcess process, WfActivityInstance activityInstance, WfWorkflowInstanceData data)
        {            
            //实体名
            string entityName = process.ActivityEntity.EntityName;

            var es = IEntitySchemaHelper.Get(entityName);

            //主键名
            string keyFieldName = es.KeyName;// process.ActivityEntity.GetKeyFieldName();

            //字段列表
            var fieldList = process.ActivityEntity.Fields.ToList();

            //签核流程
            bool isApprove = process.ProcessCategory == (int)ProcessCategory.Approve;

            if (isApprove)
            {
                SysApproveActivityData ad = context.FirstOrDefault<SysApproveActivityData>(p => p.ActivityInstanceId == activityInstance.ActivityInstanceId);
                if (ad != null)
                {
                    WfActivityEntityInstance entity = GetApproveEntityData(context,ad);
                    activityInstance.ActivityEntityInstance = entity;
                }
            }
            else
            {
                //标哥说普通流程活动实例ID和活动实体ID是一样的……
                int id = activityInstance.ActivityInstanceId;

                dynamic entityData = (new OrgManager(context)).GetEntity(entityName, id);

                if (entityData != null)
                {
                    Type entityType = entityData.GetType();

                    WfActivityEntityInstance entity = new WfActivityEntityInstance()
                    {
                        ActivityEntityInstanceId = id,
                    };

                    //属性的键值对
                    entity.ActivityEntityInstanceData = entityType.GetProperties()
                        .Where(i => i.Name != keyFieldName && !IgnoreList.Contains(i.Name))
                        .ToDictionary(i => i.Name, i =>
                        {
                            var pValue = new WfActivityFieldData();
                            try
                            {
                                pValue.Value = Convert.ToString(i.GetValue(entityData, null));
                            }
                            catch { }
                            return pValue;
                        });


                    //加上DisplayText
                    foreach (var i in entity.ActivityEntityInstanceData)
                    {
                        var field = fieldList.FirstOrDefault(q => q.FieldName.Contains(i.Key));
                        if (field != null)
                        {
                            i.Value.DisplayText = field.DisplayText;
                        }
                        else
                        {
                            i.Value.DisplayText = i.Key;
                        }
                    }

                    activityInstance.ActivityEntityInstance = entity;
                }
            }
        }
    }
}
