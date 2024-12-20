using OpenQA.Selenium;

// Email client code
public class EmailHandler
{
    private IWebDriver _driver;

    public EmailHandler(IWebDriver driver)
    {
        _driver = driver;
    }

    public string GetFakeEmailAddress()
    {
        // Site opening
        _driver.Navigate().GoToUrl("https://fakemail.net");

        // Receiving email
        string emailAddress = WaitH.WaitUntilElementClickable(_driver, By.Id("email")).Text;

        return emailAddress;
    }

    public void ConfirmEmail()
    {
        _driver.Navigate().GoToUrl("https://www.fakemail.net/");

        // Wait until the items with e-mails are visible
        var emailElements = WaitH.WaitUntilElementsVisible(_driver, By.CssSelector("#schranka .klikaciRadek"));

        foreach (var emailElement in emailElements)
        {
            if (emailElement.Text.Contains("Account confirmation", StringComparison.OrdinalIgnoreCase))
            {
                // Open the letter
                emailElement.Click();

                // Wait for the loading of the iframe with the email content
                WaitH.WaitUntilElementClickable(_driver, By.Id("iframeMail"));

                // Switch to iframe
                _driver.SwitchTo().Frame("iframeMail");

                // Wait until the “Confirm registration” button becomes available
                WaitH.WaitUntilElementClickable(_driver, By.XPath("//a[contains(text(), 'Confirm registration')]")).Click();

                // Switch to a new tab
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);

                break;
            }
        }
    }
}
