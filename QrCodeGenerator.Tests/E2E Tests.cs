using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QrCodeGenerator.Tests;
[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [SetUp]
    public async Task SetUp()
    {
        // Add a Testing User to Local Storage
        string script = @"
            localStorage.setItem('user', JSON.stringify({Name:'TestingUser', ApiKey:'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwbGFuIjoiYmFzaWMiLCJpYXQiOjE3MDIyNTk3ODd9.GFogrkfsjy2sZZLwGBzwIWz__Ib0023X5z0xXaW2piU'}));
        ";
        // Add the local storage before any of the tests start
        await Context.AddInitScriptAsync(script);
    }
    [Test, Order(1)]
    public async Task AddAndVerifyQrCodePersistence()
    {
        // Navigate to the page
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Add a new QR Code
        await Page.Locator("role=button[name='Add QR Code']").ClickAsync();
        await Page.Locator("input[type='text']").First.FillAsync("Test Static Title");
        await Page.Locator("input[type='text']").Nth(1).FillAsync("Test Static Description");
        await Page.Locator("input[type='text']").Nth(2).FillAsync("https://github.com/GusTheProgrammer/QrCodeGenerator");
        await Page.Locator("text=Static").First.CheckAsync();
        await Page.Locator("role=button[name='Add']").ClickAsync();

        // Assert that the QR Code was added successfully via the toast
        var toastLocator = Page.Locator("text=QR Code Added Successfully!");
        await toastLocator.WaitForAsync();
        Assert.IsTrue(await toastLocator.IsVisibleAsync(), "Success message not displayed.");


        // Refresh the page
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Wait for the title to become visible before asserting
        var titleLocator = Page.Locator($"text=Test Static Title");
        await titleLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });

        // Assert that the values entered in the form are now visible
        Assert.IsTrue(await Page.Locator($"text=Test Static Title").IsVisibleAsync(), "Title not found.");
        Assert.IsTrue(await Page.Locator($"text=Test Static Description").IsVisibleAsync(), "Description not found.");
        // Assert that the badge is "Static"
        var badgeLocator = Page.Locator("span:has-text('Static')");
        Assert.IsTrue(await badgeLocator.IsVisibleAsync(), "Badge does not indicate 'Static'.");

        // Click on open in new tab to verify that it opens the correct URL
        var qrCodeLinkSelector = ".mud-card-actions > a > .mud-button-root";
        var newPage = await Page.RunAndWaitForPopupAsync(async () =>
        {
            await Page.Locator(qrCodeLinkSelector).ClickAsync();
        });

        Assert.That(newPage.Url, Is.EqualTo("https://github.com/GusTheProgrammer/QrCodeGenerator"), "URL does not match the expected value.");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await newPage.CloseAsync(); // Close the new tab as we have verified the URL.

        // Click on the download button to verify that it downloads the correct file
        var download = await Page.RunAndWaitForDownloadAsync(async () =>
        {
            await Page.Locator(".mud-card-actions > button").ClickAsync();
        });

        // Verify the download
        Assert.IsNotNull(download, "No download occurred.");
        // Check if the download's suggested filename contains the title "Test Static Title"
        Assert.That(download.SuggestedFilename, Does.Contain("Test Static Title"), "Downloaded file name does not match expected title.");
    }

    [Test, Order(2)]
    public async Task EditQrCodeAndVerify()
    {
        // Navigate to the page
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Assert the QR code exists and click to edit
        var qrCodeTitleLocator = Page.Locator("role=heading[name='Test Static Title']");
        await qrCodeTitleLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        Assert.IsTrue(await qrCodeTitleLocator.IsVisibleAsync(), "QR code with the title 'Test Static Title' was not found.");
        await qrCodeTitleLocator.ClickAsync();

        // Click on edit icon
        await Page.Locator("button:nth-child(2)").ClickAsync(); // Ensure this selector correctly targets the edit button

        // Assert the edit modal is open
        var editModalLocator = Page.Locator("role=heading[name='Edit QR Code']");
        Assert.IsTrue(await editModalLocator.IsVisibleAsync(), "Edit QR Code modal was not opened.");

        // Edit the QR code title
        var firstInputLocator = Page.Locator("input[type='text']").First;
        await firstInputLocator.ClickAsync();
        await firstInputLocator.FillAsync("Edited Static Title");

        // Edit the QR code description
        var secondInputLocator = Page.Locator("input[type='text']").Nth(1);
        await secondInputLocator.ClickAsync();
        await secondInputLocator.FillAsync("Edited Static Description");

        // Click on update
        await Page.Locator("role=button[name='Update']").ClickAsync();

        // Assert the success toast message
        var successToastLocator = Page.Locator("text=QR Code Edited Successfully!");
        await successToastLocator.WaitForAsync();
        Assert.IsTrue(await successToastLocator.IsVisibleAsync(), "Success message for QR code edition was not displayed.");

        // Refresh the page to verify the changes
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Assert the edited QR code is displayed with the updated details
        var editedQrCodeTitleLocator = Page.Locator("text=Edited Static Title");
        await editedQrCodeTitleLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        Assert.IsTrue(await editedQrCodeTitleLocator.IsVisibleAsync(), "Edited QR code title was not found.");
        var editedQrCodeDescriptionLocator = Page.Locator("text = Edited Static Description");
        await editedQrCodeDescriptionLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        Assert.IsTrue(await editedQrCodeDescriptionLocator.IsVisibleAsync(), "Edited QR code description was not found.");
    }

    [Test, Order(3)]
    public async Task DeleteQrCodeAndVerify()
    {
        // Navigate to the page
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Ensure the correct QR code is present
        var qrCodeTitleLocator = Page.Locator("text=Edited Static Title");
        await qrCodeTitleLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        Assert.IsTrue(await qrCodeTitleLocator.IsVisibleAsync(), "QR code with the title 'Edited Static Title' was not found.");

        // Click on the delete icon for the selected QR code
        var deleteIconLocator = Page.Locator(".mud-card-header-actions > button").First;
        await deleteIconLocator.ClickAsync();

        // Confirm deletion by clicking on the delete button in the modal popup
        var deleteButtonLocator = Page.Locator("role=button[name='Delete']");
        await deleteButtonLocator.ClickAsync();

        // Verify the deletion success message
        var successMessageLocator = Page.Locator("text=QR Code Deleted Successfully!");
        await successMessageLocator.WaitForAsync();
        Assert.IsTrue(await successMessageLocator.IsVisibleAsync(), "Success message for QR code deletion was not displayed.");

        // Refresh the page to update the list of QR codes
        await Page.GotoAsync("https://happy-ground-0bdc11803-preview.westeurope.4.azurestaticapps.net/");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        // Assert that the "No QR Codes Found" message is displayed
        var noQrCodesLocator = Page.Locator("text=No QR Codes Found");
        await noQrCodesLocator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });
        Assert.IsTrue(await noQrCodesLocator.IsVisibleAsync(), "The message 'No QR Codes Found' was not displayed after deletion.");
    }


}

