using OpenQA.Selenium;
using System.Collections.ObjectModel;

// Waiting methods
public static class WaitH
{
    // Method for waiting until the item is available for clicking
    public static IWebElement WaitUntilElementClickable(IWebDriver driver, By by)
    {
        IWebElement element = null;
        while (true) // Infinite loop until we return an element
        {
            try
            {
                element = driver.FindElement(by);
                if (element.Displayed && element.Enabled)
                {
                    return element; 
                }
            }
            catch (NoSuchElementException)
            {
                // Element not found, continue the loop
            }
        }
    }

    // Method to wait until the list of items is visible
    public static ReadOnlyCollection<IWebElement> WaitUntilElementsVisible(IWebDriver driver, By by)
    {
        ReadOnlyCollection<IWebElement> elements = null;
        while (true) 
        {
            try
            {
                elements = driver.FindElements(by);
                if (elements != null && elements.Count > 0)
                {
                    return elements; 
                }
            }
            catch (NoSuchElementException)
            {
            
            }
        }
    }

    // Method for waiting for element visibility
    public static IWebElement WaitUntilElementVisible(IWebDriver driver, By by)
    {
        IWebElement element = null;
        while (true)
        {
            try
            {
                element = driver.FindElement(by);
                if (element.Displayed)
                {
                    return element; 
                }
            }
            catch (NoSuchElementException)
            {

            }
            catch (ElementNotVisibleException)
            {

            }
        }
    }
}
