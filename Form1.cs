    //////////////////////////////////////
   /// https://github.com/FrostUNK/   ///
  /// If you post my code somewhere, ///
 ///        please tag me :)        ///
//////////////////////////////////////
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EsetKeyGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--start-maximized");

                // Console hiding
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                using IWebDriver driver = new ChromeDriver(service, chromeOptions);

                // Temporary mail receipt
                var emailHandler = new EmailHandler(driver);
                string emailAddress = emailHandler.GetFakeEmailAddress();

                driver.Navigate().GoToUrl("https://login.eset.com/register");

                // Waiting and pressing buttons one at a time
                var languageSelector = WaitH.WaitUntilElementClickable(driver, By.CssSelector(".ltr-s8zsu"));
                languageSelector.Click();

                var englishOption = WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[data-label='languageSelector-item-en-US-radio']"));
                englishOption.Click();

                var emailField = WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[type='email']"));
                emailField.SendKeys(emailAddress);

                var continueButton = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[type='submit']"));
                continueButton.Click();

                string password = PassGen.GeneratePassword(12);
                var passwordField = WaitH.WaitUntilElementClickable(driver, By.Id("password"));
                passwordField.SendKeys(password);

                var countrySelector = WaitH.WaitUntilElementClickable(driver, By.CssSelector(".select__control"));
                countrySelector.Click();

                var options = WaitH.WaitUntilElementsVisible(driver, By.CssSelector(".select__option"));
                foreach (var option in options)
                {
                    if (option.Text.Equals("United States", StringComparison.OrdinalIgnoreCase))
                    {
                        option.Click();
                        break;
                    }
                }

                var Button2 = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[type='submit']"));
                Button2.Click();

                var resendButton = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='account-verification-email-modal-resend-email-btn']"));
                emailHandler.ConfirmEmail();

                var getStartedButton = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='home-overview-empty-add-license-btn']"));
                getStartedButton.Click();

                var esetFree = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-trial-license-card-button']"));
                esetFree.Click();

                var Button3 = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-continue-button']"));
                Button3.Click();

                var esetHome = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-ehsp-card-button']"));
                esetHome.Click();

                var Button4 = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button.css-1tz7qlj"));
                Button4.Click();

                var subscription = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-esbs-card-button']"));
                subscription.Click();

                var Button5 = WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-continue-btn']"));
                Button5.Click();

                var esetKey = WaitH.WaitUntilElementVisible(driver, By.CssSelector("div.DetailInfoSectionItem__value[data-r='license-detail-license-key'] p.css-1akdxnt"));
                return esetKey.Text;
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
