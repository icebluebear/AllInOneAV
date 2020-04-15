using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.JavDataBaseHelper;
using Model.Common;
using Model.FindModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvPlayer
{
    public partial class AvPlayer : Form
    {
        private static string imgFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static int pageIndex = 0;
        private static int pageSize = 50;
        private static PageModel pageModel;

        public AvPlayer()
        { 
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            InitDropDown();
        }

        private void AvPlayer_Load_1(object sender, EventArgs e)
        {

        }

        private void AvPlayer_Load(object sender, EventArgs e)
        {
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetPressed();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchPressed();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrePage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        private void InitDropDown()
        {
            var actress = JavDataBaseManager.GetDropDownList("Actress");
            var company = JavDataBaseManager.GetDropDownList("Company");
            var director = JavDataBaseManager.GetDropDownList("Director");
            var publisher = JavDataBaseManager.GetDropDownList("Publisher");
            var category = JavDataBaseManager.GetDropDownList("Category");

            var actressSource = new AutoCompleteStringCollection();
            actressSource.AddRange(actress);

            var companySource = new AutoCompleteStringCollection();
            companySource.AddRange(company);

            var directorSource = new AutoCompleteStringCollection();
            directorSource.AddRange(director);

            var publisherSource = new AutoCompleteStringCollection();
            publisherSource.AddRange(publisher);

            var categorySource = new AutoCompleteStringCollection();
            categorySource.AddRange(category);

            cbActress.AutoCompleteCustomSource = actressSource;
            cbActress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbActress.AutoCompleteSource = AutoCompleteSource.CustomSource;

            cbCompany.AutoCompleteCustomSource = companySource;
            cbCompany.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCompany.AutoCompleteSource = AutoCompleteSource.CustomSource;

            cbDirector.AutoCompleteCustomSource = directorSource;
            cbDirector.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbDirector.AutoCompleteSource = AutoCompleteSource.CustomSource;

            cbPublisher.AutoCompleteCustomSource = publisherSource;
            cbPublisher.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbPublisher.AutoCompleteSource = AutoCompleteSource.CustomSource;

            cbCategory.AutoCompleteCustomSource = categorySource;
            cbCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;

            pageModel = new PageModel();
            lbCurrent.Text = "0";
            lbTotal.Text = "0";
        }

        private string GenerateWhere()
        {
            string where = "";
            int year = 0;

            if (!string.IsNullOrEmpty(txtAvId.Text))
            {
                where += string.Format(" AND m.AvId LIKE '%{0}%'", txtAvId.Text);
            }

            if (!string.IsNullOrEmpty(cbActress.Text))
            {
                where += string.Format(" AND a.Actress LIKE '%{0}%'", cbActress.Text);
            }

            if (!string.IsNullOrEmpty(cbCompany.Text))
            {
                where += string.Format(" AND a.Company LIKE '%{0}%'", cbCompany.Text);
            }

            if (!string.IsNullOrEmpty(cbDirector.Text))
            {
                where += string.Format(" AND a.Director LIKE '%{0}%'", cbDirector.Text);
            }

            if (!string.IsNullOrEmpty(cbPublisher.Text))
            {
                where += string.Format(" AND a.Publisher LIKE '%{0}%'", cbPublisher.Text);
            }

            if (!string.IsNullOrEmpty(cbCategory.Text))
            {
                where += string.Format(" AND a.Category LIKE '%{0}%'", cbCategory.Text);
            }

            if (!string.IsNullOrEmpty(txtYear.Text))
            {
                if (int.TryParse(txtYear.Text, out year))
                {
                    where += string.Format(" AND a.ReleaseDate>='{0}' AND a.ReleaseDate<'{1}'", year + "-01-01", (year + 1) + "-01-01");
                }
            }

            if (!int.TryParse(txtPageSize.Text, out pageSize))
            {
                txtPageSize.Text = "50";
                pageSize = 50;
            }

            return where;
        }

        private void ResetPressed()
        {
            txtAvId.Text = "";
            cbActress.Text = "";
            cbCompany.Text = "";
            cbDirector.Text = "";
            cbPublisher.Text = "";
            cbCategory.Text = "";
            txtPageSize.Text = "50";
            rbDateDesc.Checked = true;
            rbDateAsc.Checked = false;
            pageSize = 50;
            pageIndex = 0;
        }

        private void SearchPressed()
        {
            var where = GenerateWhere();
            var order = rbDateAsc.Checked ? "ASC" : "DESC";

            var avs = GetAVs(where, order);
            ShowAVs(avs);
        }

        private List<AVViewModel> GetAVs(string where, string order)
        {
            var avs = FindDataBaseManager.GetAllViewModel(where, pageIndex, pageSize, order, ref pageModel);
            pageModel.TotalPage = (pageModel.Total % pageSize) == 0 ? (pageModel.Total / pageSize) : (pageModel.Total / pageSize) + 1;
            lbCurrent.Text = "1";
            lbTotal.Text = pageModel.TotalPage + "";

            foreach (var av in avs)
            {
                var img = imgFolder + av.AvId + av.Name + ".jpg";
                var file = av.Location + "/" + av.FileName;

                if (File.Exists(img))
                {
                    av.Img = img;
                }

                if (File.Exists(file))
                {
                    FileInfo f = new FileInfo(file);
                    av.FileLength = f.Length;
                    av.FileSize = FileSize.GetAutoSizeString(f.Length, 2);
                    av.FileLocation = file;
                }
            }

            return avs;
        }

        private void ShowAVs(List<AVViewModel> avs)
        {
            panel3.Controls.Clear();
            var count = avs.Count;

            var outerWidth = panel3.Width - 50;
            var outerHeight = panel3.Height;

            var singleWidth = (int)(outerWidth / 3);
            var singleHeight = (int)(singleWidth / 1.5) + txtAvId.Height;

            int x = 2, y = 2;

            while (count > 0)
            {
                count -= 3;

                foreach(var av in avs.Take(3))
                {
                    ListItem item = new ListItem(av.Img, av.Name, av.AvId, av.FileLocation);
                    item.Width = singleWidth;
                    item.Height = singleHeight;

                    item.Location = new Point(x, y);

                    panel3.Controls.Add(item);

                    x += singleWidth + 5;
                }

                x = 2;
                y += singleHeight + 2;

                avs = avs.Skip(3).ToList();
            }
        }

        private void PrePage()
        {
            if (pageIndex > 0)
            {
                pageIndex--;

                SearchPressed();
                UpdatePageInfo();
            }
        }

        private void NextPage()
        {
            if (pageIndex + 1 < pageModel.TotalPage)
            {
                pageIndex++;

                SearchPressed();
                UpdatePageInfo();
            }
        }

        private void UpdatePageInfo()
        {
            lbCurrent.Text = (pageIndex + 1) + "";
            lbTotal.Text = pageModel.TotalPage + "";
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
