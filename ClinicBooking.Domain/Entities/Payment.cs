using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Payment
    {
        public int Id { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public decimal Amount { get; set; }
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }

        public PaymentStatus Status { get; set; }

        public string? VerificationCodeHash { get; set; }
        public DateTime? VerificationSentAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }

