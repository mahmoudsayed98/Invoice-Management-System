using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages
{
    public class IndexModel : PageModel
    {
        public Dictionary<string, int> revenueSubmitted;
        public Dictionary<string, int> revenueApproved;
        public Dictionary<string, int> revenueRejected;
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            InitDict(ref revenueSubmitted);
            InitDict(ref revenueApproved);
            InitDict(ref revenueRejected);

            var invoices = _context.Invoice.ToList();

            foreach ( var invoice in invoices)
            {
                switch(invoice.Status)
                {
                    case InvoiceStatus.Submitted:
                        revenueSubmitted[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                    case InvoiceStatus.Approved:
                        revenueApproved[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                    case InvoiceStatus.Rejected:
                        revenueRejected[invoice.InvoiceMonth] += (int)invoice.InvoiceAmount;
                        break;
                    default:
                        break;

                }
            }

        }
        
        private void InitDict(ref Dictionary<string, int> dict)
        {
            dict = new Dictionary<string, int>()
            {
                { "jan",0 },
                { "feb",0 },
                { "mar",0 },
                { "apr",0 },
                { "may",0 },
                { "jun",0 },
                { "jul",1 },
                { "aug",0 },
                { "sep",0 },
                { "oct",0 },
                { "nov",0 },
                { "dec",0 }
           };
            
        }
    }
}