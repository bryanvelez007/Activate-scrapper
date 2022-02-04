using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace vbEngC
{
public partial class Form1 : Form
{
    public Form1()
    {
            InitializeComponent();
    }

    IFirebaseConfig fcon = new FirebaseConfig()
    {
        AuthSecret = "2ckBr8dN9WDfdAazDTO7KbfbBtUlgyKQ6yAJKKqf",
        BasePath = "https://test-automatizacion-default-rtdb.firebaseio.com/"
    };

    IFirebaseClient client;

        private void Form1_Load(object sender, EventArgs e)
    {
        try 
        {
            client = new FireSharp.FirebaseClient(fcon);
            updateLister();
        }
        catch
        {
            MessageBox.Show("there was problem in the internet.");
        }
    }

    private async void updateLister()
    {

        Automatizacion testing = new Automatizacion();

        EventStreamResponse response = await client.OnAsync("startScript/", 
        added: (s, args, context) =>
        {
            if (args.Data == "Started")
            {
                var getCompanyNames = client.Get("listCompany/");
              //  testing.scrapingPhoneNumbers();
               Student std = getCompanyNames.ResultAs<Student>();
               MessageBox.Show(std.names);
            }
        },
        changed: (s, args, context) =>
        {
            if(args.Data == "Started")
            {
                var getCompanyNames = client.Get("listCompany/");
              //  testing.scrapingPhoneNumbers();
                //  Student std = result.ResultAs<Student>();
                //  MessageBox.Show(std.names);
            }
        },
        removed: (s, args, context) =>
        {
            ///MessageBox.Show(""+args);
        });

    }

    private void InsertBtn_Click(object sender, EventArgs e)
    {
        Student std = new Student()
        {
            names = nameTbox.Text,
            RollNo = rollTbox.Text,
            Grade = gradeTobx.Text,
            Section = secTbox.Text
        };
        var setter = client.Set("StudentList/"+rollTbox.Text,std);
        MessageBox.Show("data inserted successfully");
    }

    private void SelectBtn_Click(object sender, EventArgs e)
    {
        var result = client.Get("StudentList/" + rollTbox.Text);
        Student std = result.ResultAs<Student>();
        nameTbox.Text = std.names;
        gradeTobx.Text = std.Grade;
        secTbox.Text = std.Section;
        MessageBox.Show("data retrieved successfully");
    }

    private void UpdateBtn_Click(object sender, EventArgs e)
    {
        Student std = new Student()
        {
            names = nameTbox.Text,
            RollNo = rollTbox.Text,
            Grade = gradeTobx.Text,
            Section = secTbox.Text
        };
        var setter = client.Update("StudentList/" + rollTbox.Text, std);
        MessageBox.Show("data inserted successfully");
    }

    private void DeleteBtn_Click(object sender, EventArgs e)
    {
        var result = client.Delete("StudentList/" + rollTbox.Text);
        MessageBox.Show("data deleted successfully");
    }
}
}
