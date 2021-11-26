using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;

namespace WYNK.Data.Common
{
    public class StockCheck
    {
        public static StockCheckModel MedicalPrescription(ItemBatch res, decimal totalQuantity, decimal requestedQuantity, decimal StillNeededQuantity)
        {
            var stockCheck = new StockCheckModel();

            res.LockedQuantity = res.LockedQuantity != null ? res.LockedQuantity : 0;

            stockCheck.ActualTakenFromBatch = 0;

            stockCheck.AllotedBatch = new AllotedBatch()
            {
                DrugId = res.ItemID,
                balanceQty = res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity),
                CreatedUTC = res.CreatedUTC,
                ExpiryDate = res.ItemBatchExpiry,
                GoingToIssue = IssuingQty(res.ItemBatchBalnceQty, res.LockedQuantity, StillNeededQuantity),
                itemBatchNo = res.ItemBatchNumber,
            };

            if (Math.Abs(Convert.ToDecimal(res.ItemBatchBalnceQty - res.LockedQuantity)) <= StillNeededQuantity)
            {
                stockCheck.ActualTakenFromBatch = Math.Abs((StillNeededQuantity - (res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity))) - StillNeededQuantity);
            }
            else
            {
                stockCheck.ActualTakenFromBatch = Math.Abs(((res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity)) - StillNeededQuantity) - (res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity)));
            }

            stockCheck.totalQuantity = totalQuantity + (res.ItemBatchBalnceQty - Convert.ToDecimal(res.LockedQuantity));

            stockCheck.StillNeededQuantity = StillNeededQuantity - stockCheck.ActualTakenFromBatch;

            return stockCheck;
        }


        public static decimal IssuingQty(decimal balanceQty, int? LockedQuantity, decimal StillNeededQuantity)
        {
            decimal res = 0;

            LockedQuantity = LockedQuantity != null ? LockedQuantity : 0;

            if (Math.Abs(Convert.ToDecimal(balanceQty - LockedQuantity)) <= StillNeededQuantity)
            {
                res = Math.Abs((StillNeededQuantity - (balanceQty - Convert.ToDecimal(LockedQuantity))) - StillNeededQuantity);
            }
            else
            {
                res = Math.Abs(((balanceQty - Convert.ToDecimal(LockedQuantity)) - StillNeededQuantity) - (balanceQty - Convert.ToDecimal(LockedQuantity)));
            }

            return res;
        }
    }
}
