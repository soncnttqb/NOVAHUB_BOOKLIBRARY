using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Business.Utilities;

namespace BookLibrary.UcControls
{
    public partial class UcPaging : UserControl
    {
        public delegate void PagingHandler(int pageIndex);
        public event PagingHandler ExecutePaging;

        public int TotalRecord { get; set; }
        public int PageSize { get { return int.Parse(ConfigurationManager.AppSettings[Constants.ConfigKey.PageSize].ToString()); } }
        public int PageIndex { get; set; }
        public int TotalPage
        {
            get
            {
                if (TotalRecord == 0) return 0;
                if (TotalRecord % PageSize == 0)
                    return TotalRecord / PageSize;
                return TotalRecord / PageSize + 1;
            }
        }
        public UcPaging()
        {
            InitializeComponent();
        }

        private void UcPaging_Load(object sender, EventArgs e)
        {
            PageIndex = 1;
        }
        public void Config()
        {
            btnFirst.Enabled = btnPrevious.Enabled = PageIndex > 1 && TotalPage > 1;
            btnNext.Enabled = btnLast.Enabled = PageIndex < TotalPage && TotalPage > 1;
            lblTotalPage.Text = $"/{TotalPage} {(TotalPage > 1 ? "Pages" : "Page")}";
        }

        public void ResetPaging()
        {
            numPageIndex.Value = 1;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            numPageIndex.Value = 1;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            numPageIndex.Value = PageIndex - 1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            numPageIndex.Value = PageIndex + 1;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            numPageIndex.Value = TotalPage;
        }

        private void numPageIndex_ValueChanged(object sender, EventArgs e)
        {
            PageIndex = (int)numPageIndex.Value;
            ExecutePaging?.Invoke(PageIndex);
        }
    }
}
