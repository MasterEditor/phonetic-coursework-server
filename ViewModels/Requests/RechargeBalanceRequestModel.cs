using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Coursework_Server.ViewModels.Requests
{
    public class RechargeBalanceRequestModel
    {
        [Required(ErrorMessage = "Номер карты не может быть пустым")]
        [CreditCard(ErrorMessage = "Неверный номер кредитной карты")]
        public string CardNumber { get; set; }

        [Range(1, 99, ErrorMessage = "Некорректный формат месяца")]
        public int CardMonth { get; set; }

        [Range(1, 99, ErrorMessage = "Некорректный формат года")]
        public int CardYear { get; set; }

        [Range(1, 999, ErrorMessage = "Некорректный формат CVV")]
        public int CardCVV { get; set; }

        [Range(1, 999999, ErrorMessage = "Введите корректную сумму пополнения")]
        public double Value { get; set; }
    }
}
