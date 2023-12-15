using BikeDB.bikeDB;
using System.ComponentModel;
using System.Windows.Forms;

namespace BikeDB
{
    public partial class Form1 : Form
    {
        public List<Bike> bikes = new List<Bike>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshData();

            using (var db = new BikeDbContext())
            {
                bikes = db.Bikes.ToList();

                int count = 0;
                foreach (var bike in bikes)
                {
                    Button button = new Button();
                    button.Name = bike.Id.ToString();
                    button.Text = bike.Id + "ȣ��";
                    button.Tag = bike.Used;
                    button.BackColor = bike.Used ? Color.Red : Color.Green;
                    button.Location = new Point(20 + (count++ * 130), 30);
                    button.Size = new Size(120, 120);
                    button.Click += (s, e) =>
                    {
                        if (!bike.Used)
                        {
                            if (MessageBox.Show(bike.Id + "ȣ�⸦ �뿩�Ͻðڽ��ϱ�?", "Ȯ��", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var db2 = new BikeDbContext())
                                {
                                    db2.RentalHistories.Add(new RentalHistory()
                                    {
                                        BikeId = bike.Id,
                                        RentalTime = DateTime.Now
                                    });
                                    db2.Bikes.First(p => p.Id == bike.Id).Used = true;
                                    bike.Used = true;
                                    button.BackColor = Color.Red;
                                    db2.SaveChanges();
                                }
                                RefreshData();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show(bike.Id + "ȣ�⸦ �ݳ��Ͻðڽ��ϱ�?", "Ȯ��", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var db2 = new BikeDbContext())
                                {
                                    var item = db2.RentalHistories.Where(p => p.ReturnTime == null && p.BikeId == bike.Id).FirstOrDefault();

                                    if (item != null)
                                        item.ReturnTime = DateTime.Now;

                                    db2.Bikes.First(p => p.Id == bike.Id).Used = false;
                                    bike.Used = false;
                                    button.BackColor = Color.Green;
                                    db2.SaveChanges();
                                }
                                RefreshData();
                            }
                        }
                    };
                    this.Controls.Add(button);
                }
            }
        }

        void RefreshData()
        {
            using (var db = new BikeDbContext())
            {
                dataGridView1.DataSource = db.RentalHistories
                    .Select(p => new RentalDataView()
                    {
                        Id = p.Id,
                        RentalTime = p.RentalTime,
                        ReturnTime = p.ReturnTime
                    }).ToList();
            }
        }
    }

    public class RentalDataView
    {
        [DisplayName("ȣ��")]
        public int Id { get; set; }
        [DisplayName("�뿩�ð�")]
        public DateTime RentalTime { get; set; }
        [DisplayName("�ݳ��ð�")]
        public DateTime? ReturnTime { get; set; }
    }
}

