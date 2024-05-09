using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watsons.Common;
using Watsons.Common.EmailHelpers;
using Watsons.Common.EmailHelpers.DTO;
using Watsons.TRV2.DA.CashManage;
using Watsons.TRV2.DA.TR.Repositories;

namespace Watsons.TRV2.Services.Portal
{
    public class JobService : IJobService
    {
        private readonly ITrOrderBatchRepository _trOrderBatchRepository;
        private readonly ICashManageRepository _cashManageRepository;
        private readonly EmailHelper _emailHelper;
        public JobService(
            ITrOrderBatchRepository trOrderBatchRepository,
            ICashManageRepository cashManageRepository,
            EmailHelper emailHelper)
        {
            _trOrderBatchRepository = trOrderBatchRepository;
            _cashManageRepository = cashManageRepository;
            _emailHelper = emailHelper;
        }

        public async Task<ServiceResult<bool>> EmailNotifyStoreOrderPending()
        {
            var orderPendingList = await _trOrderBatchRepository.PendingList();
            var storeIds = orderPendingList.Select(o => o.StoreId).ToList();
            if(storeIds == null || storeIds.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            var roles = new List<string>() { "ASOM", "RSOM"};
            var userStoreList = await _cashManageRepository.UserStoreIds(storeIds, roles);
            if(userStoreList == null || userStoreList.Count() <= 0)
            {
                return ServiceResult<bool>.Fail("No user found.");
            }

            var userStoreDict = userStoreList.GroupBy(u => u.Email).ToDictionary(u => u.Key, u => u.Select(v => v.StoreId).ToList());
            if(userStoreDict == null || userStoreDict.Count() <= 0)
            {
                return ServiceResult<bool>.Success(true);
            }

            foreach(var userStore in userStoreDict)
            {
                var email = userStore.Key;
                var storeIdList = userStore.Value;
                var storeIdStr = string.Join(",", storeIdList);
                var subject = $"Tester Store Pending Order Notification";
                var body = $@"Tester Store Order Pending : {storeIdStr} <br/><br/><br/>";
                var emailParams = new SendEmailBySpParams() 
                {
                    Recipients = new List<string>() { email },
                    Subject = subject,
                    Body = body
                };

                await _emailHelper.SendEmailBySp(emailParams);
            }

            return ServiceResult<bool>.Success(true);
        }
    }
}
