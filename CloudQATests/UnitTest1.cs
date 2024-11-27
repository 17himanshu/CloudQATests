using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace CloudQATests
{
    public class PracticeFormTests
    {
        private IWebDriver? driver;
        private WebDriverWait? wait;
        private const string URL = "https://app.cloudqa.io/home/AutomationPracticeForm";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestPracticeFormFields()
        {
            try
            {
                Assert.That(driver, Is.Not.Null, "WebDriver was not initialized");
                driver!.Navigate().GoToUrl(URL);

                // Test data
                string firstName = "John";
                string lastName = "Doe";
                string email = "john.doe@example.com";
                string mobile = "1234567890";

                // Find elements using correct IDs from HTML
                IWebElement firstNameField = wait!.Until(d => d.FindElement(By.Id("fname")));
                IWebElement lastNameField = wait!.Until(d => d.FindElement(By.Id("lname")));
                IWebElement emailField = wait!.Until(d => d.FindElement(By.Id("email")));
                IWebElement mobileField = wait!.Until(d => d.FindElement(By.Id("mobile")));

                // Clear existing values
                firstNameField.Clear();
                lastNameField.Clear();
                emailField.Clear();
                mobileField.Clear();

                // Fill out the fields
                firstNameField.SendKeys(firstName);
                lastNameField.SendKeys(lastName);
                emailField.SendKeys(email);
                mobileField.SendKeys(mobile);

                // Select gender
                IWebElement maleRadio = wait.Until(d => d.FindElement(By.Id("male")));
                maleRadio.Click();

                // Select hobbies
                IWebElement readingHobby = wait.Until(d => d.FindElement(By.Id("Reading")));
                if (!readingHobby.Selected)
                {
                    readingHobby.Click();
                }

                // Verify the entered values
                Assert.Multiple(() =>
                {
                    Assert.That(firstNameField.GetAttribute("value"), Is.EqualTo(firstName), "First name verification failed");
                    Assert.That(lastNameField.GetAttribute("value"), Is.EqualTo(lastName), "Last name verification failed");
                    Assert.That(emailField.GetAttribute("value"), Is.EqualTo(email), "Email verification failed");
                    Assert.That(mobileField.GetAttribute("value"), Is.EqualTo(mobile), "Mobile verification failed");
                    Assert.That(maleRadio.Selected, Is.True, "Gender selection failed");
                    Assert.That(readingHobby.Selected, Is.True, "Hobby selection failed");
                });
            }
            catch (Exception ex)
            {
                // Take screenshot on failure
                if (driver != null)
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile("TestFailure.png");
                }
                Assert.Fail($"Test failed with error: {ex.Message}");
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}