# QR Code Generator 

### QR Code Management System

Welcome to the QR Code Generator, a simple tool for creating and managing your QR codes. This project utilizes ASP.NET Core Identity for authentication and uses SQLite as the database engine.

### Technologies Used

- **ASP.NET Core 7**
- **SQLite Database**
- **MudBlazor**

### Static vs. Dynamic QR Codes

Static QR Codes are fixed, meaning the data encoded in the QR code does not change once it's created. These are ideal for scenarios where the information is permanent, like an email address or a standard URL.

Dynamic QR Codes, on the other hand, are flexible and can be updated or changed even after printing. They're perfect for marketing campaigns where you might need to update the URL or other data.

### How to Use the QR Code Generator

Start by navigating to the **Add QR Code** button to create a new QR code. Fill in the details, choose the type (static or dynamic), and hit save. Your new QR code will appear in the grid below.

To edit or delete a QR code, use the pencil icon (✏️) or the trash can icon (🗑️) on each QR code card. For dynamic QR codes, you can update the destination URL at any time without changing the QR code itself.

Use the filter bar to quickly find QR codes by name or ID. To download or navigate to the QR code's link, use the download icon (📥) or the open-in-browser icon (🌐) respectively.