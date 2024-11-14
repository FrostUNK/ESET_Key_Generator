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

        // Handle key quantity change
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Get the quantity from the numeric up/down control
            int quantity = (int)numericUpDown1.Value;
            textBox1.Text = $"Generate {quantity} keys";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Disable the button and update the text while generating keys
            button1.Enabled = false;
            textBox1.Text = "Generating...";
            button1.Text = "Wait...";

            int quantity = (int)numericUpDown1.Value;
            var tasks = new Task<string>[quantity];

            // Create tasks for key generation
            for (int i = 0; i < quantity; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    return await GetActivationKeyAsync();
                });
            }

            var keys = await Task.WhenAll(tasks);

            // Handle key results
            if (keys.Length == 1)
            {
                // If only one key is generated, display it on screen
                textBox1.Text = keys[0];
            }
            else
            {
                // If multiple keys are generated, save them to a file and display the path
                string filePath = "eset-keys.txt";
                File.WriteAllLines(filePath, keys);
                textBox1.Text = $"Keys saved to {filePath}";

                OpenFile(filePath);
            }

            // Restore button state
            button1.Enabled = true;
            button1.Text = "Generate!";
        }

        // Generate keys asynchronously
        private async Task<string[]> GenerateActivationKeysAsync(int quantity)
        {
            var tasks = new Task<string>[quantity];

            // Create a list of tasks to run in parallel
            for (int i = 0; i < quantity; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    return await GetActivationKeyAsync();
                });
            }

            // Wait for all tasks to complete and return the results
            return await Task.WhenAll(tasks);
        }

        // Get a single activation key asynchronously
        private async Task<string> GetActivationKeyAsync()
        {
            return await Task.Run(() =>
            {
                // Start ChromeDriver with headless mode and hide the command prompt window
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--start-maximized");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                using IWebDriver driver = new ChromeDriver(service, chromeOptions);

                // Get a temporary email address
                string emailAddress = new EmailHandler(driver).GetFakeEmailAddress();

                driver.Navigate().GoToUrl("https://login.eset.com/register");

                // Wait for and click buttons one at a time
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='languageSelector-toggle-btn']")).Click();
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[data-label='languageSelector-item-en-US-radio']")).Click();
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("input[type='email']")).SendKeys(emailAddress);
                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[type='submit']")).Click();

                // Enter password for the account
                WaitH.WaitUntilElementClickable(driver, By.Id("password")).SendKeys(PassGen.GeneratePassword(12));

                WaitH.WaitUntilElementClickable(driver, By.CssSelector(".select__control")).Click();

                // Select the country
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

                // Confirm email
                new EmailHandler(driver).ConfirmEmail();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='home-overview-empty-add-license-btn']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-trial-license-card-button']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='license-fork-slide-continue-button']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-ehsp-card-button']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[aria-disabled='false']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-esbs-card-button']")).Click();

                WaitH.WaitUntilElementClickable(driver, By.CssSelector("button[data-label='subscription-choose-trial-continue-btn']")).Click();

                // Return the generated activation key
                return WaitH.WaitUntilElementVisible(driver, By.CssSelector("div[data-label='license-detail-license-key'] p")).Text;
            });
        }

        // Close the WebDriver when the program is closed
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Kill all ChromeDriver processes to avoid hanging
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }
            base.OnFormClosing(e);
        }

        // Open the GitHub project page when the link is clicked
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/FrostUNK/ESET_Key_Generator") { UseShellExecute = true });
        }

        private void OpenFile(string filePath)
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}
