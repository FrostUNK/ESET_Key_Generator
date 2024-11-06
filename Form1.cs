    //////////////////////////////////////
   /// https://github.com/FrostUNK/   ///
  /// If you post my code somewhere, ///
 ///        please tag me :)        ///
//////////////////////////////////////
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace EsetKeyGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Button settings
            button1.Enabled = false;
            textBox1.Text = "Generating...";
            button1.Text = "Wait...";

            // Get the key
            string activationKey = await GetActivationKeyAsync();
            textBox1.Text = activationKey;

            button1.Enabled = true;
            button1.Text = "Generate!";
        }

        private async Task<string> GetActivationKeyAsync()
        {
            return await Task.Run(() =>
            {
                // Start ChromeDriver
                var chromeOptions = new ChromeOptions();
                //chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--start-maximized");

                // Console hiding
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                using IWebDriver driver = new ChromeDriver(service, chromeOptions);

                // Temporary mail receipt
                string emailAddress = new EmailHandler(driver).GetFakeEmailAddress();

                driver.Navigate().GoToUrl("https://login.eset.com/register");

                // Waiting and pressing buttons one at a time
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='languageSelector-toggle-btn']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[data-label='languageSelector-item-en-US-radio']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[type='email']")).SendKeys(emailAddress);
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[type='submit']")).Click();
                
                // Account password
                WaitH.WaitUntilElementClickable(driver, By.Id("password")).SendKeys(PassGen.GeneratePassword(12));
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector(".select__control")).Click();

                // Select Country
                var options = WaitH.WaitUntilElementsVisible(driver, By.CssSelector(".select__option"));
                foreach (var option in options)
                {
                    if (option.Text.Equals("United States", StringComparison.OrdinalIgnoreCase))
                    {
                        option.Click();
                        break;
                    }
                }

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[type='submit']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='account-verification-email-modal-resend-email-btn']"));
                
                // Confirm Email
                new EmailHandler(driver).ConfirmEmail();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='home-overview-empty-add-license-btn']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-trial-license-card-button']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-continue-button']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-ehsp-card-button']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[aria-disabled='false']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-esbs-card-button']")).Click();
                
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-continue-btn']")).Click();
                
                return WaitH.WaitUntilElementVisible(driver, By.CssSelector("div[data-label='license-detail-license-key'] p")).Text;
            });
        }

        // Close WebDriver when closing the program
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            base.OnFormClosing(e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/FrostUNK/ESET_Key_Generator") { UseShellExecute = true });
        }

    }
}
