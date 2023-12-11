# QR Code Generator

## QR Code Management System

Welcome to the QR Code Generator, a simple tool for creating and managing your QR codes. This application uses local storage to save your unique API key and QR code data.

## Technologies Used

- **Blazor WASM Standlone App (.NET 8)**
- **Local Storage for API Key and Data Persistence**
- **MudBlazor**

## Data Management

Instead of utilizing a database like SQLite, this application stores the API key and QR code data directly in the browser's local storage. This approach ensures that your data remains readily accessible between sessions.

### Features

- **Local Storage**: Stores your API key and QR code data in the browser, making it easily accessible across sessions.
- **Data Export**: There is a feature that allows you to export your data to a file. This can be useful if you need to transfer data to another device or simply want to back up your data.
- **Data Import**: If your local storage data gets wiped or if you're migrating to a new device, the application provides a function to import your data from a previously exported file.

## Static vs. Dynamic QR Codes

Static QR Codes are fixed, meaning the data encoded in the QR code does not change once it's created. These are ideal for scenarios where the information is permanent, like an email address or a standard URL.

Dynamic QR Codes, on the other hand, are flexible and can be updated or changed even after printing. They're perfect for marketing campaigns where you might need to update the URL or other data.

## How to Use the QR Code Generator

Start by navigating to the **Add QR Code** button to create a new QR code. Fill in the details, choose the type (static or dynamic), and hit save. Your new QR code will appear in the grid below.

To edit or delete a QR code, use the pencil icon (✏️) or the trash can icon (🗑️) on each QR code card. For dynamic QR codes, you can update the destination URL at any time without changing the QR code itself.

Use the filter bar to quickly find QR codes by name or ID. To download or navigate to the QR code's link, use the download icon (📥) or the open-in-browser icon (🌐) respectively.

## Data Export and Import

In the event that your local storage data is lost, or if you are switching to a new device, the application includes features to export and import your data:

- **Export Data**: You can save your API key and QR code data to a file. This is useful for backing up your data or preparing to move it to another device.
- **Import Data**: If you need to restore your data or set up the application on a new device, you can import your data from the file you exported.

This approach ensures that you retain control over your data and can manage it according to your needs.
