# 🚢 FreightWise — Smart Freight Quotation & Booking Portal

A lightweight, enterprise-ready logistics quotation and freight booking web application built using **ASP.NET Core Web API**, **Entity Framework Core (In-Memory)**, and a responsive **Bootstrap 5** frontend.

---

## 📌 Features

- **Instant Rate Engine:** Real-time dynamic quotation generation based on POL/POD port pairings, container specs (20ft, 40ft HC, LCL), and weight.
- **Detailed Tariff Breakdown:** Itemized billing showing Base Ocean Freight, Bunker Adjustment Factor (BAF), Terminal Handling Charges (THC), and estimated transit days.
- **1-Click Booking System:** Converts approved quotes into confirmed bookings with auto-generated reference IDs (`BKG-2026-XXXX`).
- **Operational Dashboard:** Live booking overview tracking shipper details, route summary, container type, and current status.
- **Zero-Dependency Setup:** Runs out of the box using EF Core In-Memory persistence—no external database installation needed.

---

## 🛠️ Tech Stack

- **Backend:** C# / .NET 8.0, ASP.NET Core Web API
- **Data Layer:** Entity Framework Core In-Memory Provider
- **Frontend:** HTML5, CSS3, JavaScript (Fetch API), Bootstrap 5
- **Tooling:** Visual Studio Code, Git

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/krishnamani-yadav/FreightWise-Freight-Quotation-Portal.git](https://github.com/krishnamani-yadav/FreightWise-Freight-Quotation-Portal.git)
   cd FreightWise-Freight-Quotation-Portal/FreightWiseApi