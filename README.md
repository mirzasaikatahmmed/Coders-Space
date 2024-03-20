# Coder's Space

Welcome to Coder's Space! Your ultimate one-stop solution for enhancing your coding experience.

## Objectives

Introducing "Coder's Space," an innovative software tool meticulously designed to be the ultimate one-stop solution for coders, catering to their diverse needs and significantly enhancing their daily coding experiences. Developed using the robust Visual Studio and powered by the versatile C# programming language, Coder's space is a Windows Forms Application (WFA) that seamlessly integrates with a dynamic database.

This comprehensive platform strives to boost coder productivity by consolidating essential resources into a centralized hub. Coder's Space not only provides direct access to key platforms such as GitHub, LinkedIn, and problem-solving sites but also incorporates features to track users' daily progress and facilitate continuous learning through curated courses.

In addition to its primary functionalities, Coder's Space boasts a vibrant discussion forum where users can engage with fellow coders, seek advice, and share their wealth of knowledge. The software goes above and beyond by offering a customizable dashboard, allowing users to tailor their experience by prioritizing and arranging the resources most relevant to their daily workflow. Whether you're a novice or a seasoned professional, Coder's Space ensures accessibility for coders at all skill levels.

In essence, Coder's Space redefines the coding landscape, providing a centralized platform that streamlines the process of staying informed and connected in the dynamic world of coding. This indispensable tool empowers coders to stay organized, connected, and well-informed, making it an invaluable resource for individuals seeking to excel in their coding endeavors.

## Scope

The scope of the project will include the development of a Windows Forms Application (WFA) based software tool, which will include a user-friendly interface for accessing various resources. The tool will allow users to log in, create profiles, and track their daily work. The database will store user information, course information, and other relevant data.

## Features

The Coder's Space software tool will offer several key features to its users:

1. **Centralized Platform**: Access to various resources including GitHub, LinkedIn, and problem-solving sites.
2. **Progress Tracking**: Users can track their daily work and progress.
3. **Course Enrollment**: Take courses offered by verified tutors and receive certificates upon completion.
4. **Discussion Forum**: Interact with fellow coders, ask questions, and share knowledge.
5. **Customizable Dashboard**: Personalize your experience by choosing and arranging resources.
6. **Inclusivity**: Accessible to coders of all levels, from beginners to experienced professionals.

## System Features

### Regular User:

- **Registration and Login**: Create an account and login with basic credentials.
- **Access to Basic Features**: Browse content, perform basic search, and view public information.
- **Limited Permissions**: Restricted permissions, cannot access or modify advanced settings.
- **Free Membership**: Typically have a free membership with basic functionalities.

### Premium User:

- **Subscription and Payment**: Subscribe to a paid membership plan, requiring payment.
- **Enhanced Features**: Access to premium content, advanced search filters, and additional features.
- **Priority Support**: Receive priority customer support for quicker issue resolution.

### Admin:

- **User Management**: Manage user accounts, including creation, modification, or deactivation.
- **Content Control**: Control and moderate content to ensure compliance with guidelines.
- **System Configuration**: Configure system settings, security parameters, and access controls.
- **Issue Resolution**: Handle escalated support issues and resolve conflicts.
- **Analytics and Reporting**: Monitor user activities, system performance, and relevant metrics.
- **Security Management**: Implement measures to ensure system security and prevent unauthorized access.
- **Feature Development**: Plan and implement new features or improvements.

## Technology Stack

- **Programming Language**: C#
- **User Interface**: GunaUI
- **Database**: SQL Server Management Studio
- **Tools**: Visual Studio 2022, SQL Server Management Studio 22

## How to Run the Project

To run Coder's Space on your local machine, follow these steps:

1. **Clone the Repository**: 
   ```
   git clone https://github.com/mirzasaikatahmmed/Coders-Space.git
   ```

2. **Open Solution in Visual Studio**: 
   Open Visual Studio 2022 and navigate to the directory where you cloned the repository. Double click on the solution file (`.sln`) to open the project in Visual Studio.

3. **Set up Database**:
   - Open SQL Server Management Studio (SSMS) 22.
   - Connect to your SQL Server instance.
   - Execute the SQL scripts provided in the `Database Scripts` folder to create and set up the database schema.

4. **Configure Connection String**:
   - In the Visual Studio solution, locate the `app.config` file in the project.
   - Update the connection string with your SQL Server instance details.

5. **Build and Run**:
   - Build the solution in Visual Studio.
   - Press `F5` or click on the "Start" button to run the application.
   
6. **Explore Coder's Space**:
   - Once the application is running, you can register as a new user or log in with existing credentials.
   - Explore the features of Coder's Space, including accessing resources, tracking progress, and engaging with the community.


## Acknowledgments

We would like to express our gratitude to MD. NAZMUL HOSSAIN SIR, Lecturer, Computer Science, AIUB, for guiding us through the development of this project as part of our course curriculum.
## License

This project is licensed under the [GPL-3.0 license](LICENSE).

Feel free to contribute and make Coder's Space even better! If you encounter any issues or have suggestions, please open an [issue](https://github.com/mirzasaikatahmmed/Coders-Space/issues).
