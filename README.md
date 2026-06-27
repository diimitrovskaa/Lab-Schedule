# Lab Schedule 📅

ASP.NET Core MVC Project

### Developed by: Rosana Dimitrovska, Maja Poposka and Dusica Velkova
---

# Македонски / English


# 1. Опис на апликацијата

Апликацијата **Lab Schedule** претставува веб систем за автоматизирано креирање, организирање и управување со распоред за лабораториски вежби.

Главната идеја на апликацијата е да овозможи едноставен и прегледен начин за организација на наставните активности преку поврзување на предмети, лаборатории, студенти и временски термини.

Со користење на апликацијата корисникот може:

- Да креира и управува со семестри
- Да додава и отстранува предмети
- Да дефинира број на студенти
- Да додава лаборатории со различен капацитет
- Да креира распоред според ден и време
- Да следи статистика за распределба
- Овозможува зачувување и извезување на лабораторискиот распоред во PDF и Excel формати

За подобро корисничко искуство имплементирани се:

- Модерен и чист дизајн
- Responsive интерфејс
- Dark Mode
- Интерактивни табели
- Автоматско зачувување во база на податоци
- Генерирање PDF и Excel документи

Апликацијата е развиена со ASP.NET Core MVC архитектура, каде што логиката, приказот и податоците се поделени во посебни компоненти.
---

# 2. Упатство за користење и функционалности на апликацијата
## 2.1 Главен прозорец – контролен панел

При стартување на апликацијата корисникот пристапува до главниот контролен панел, кој претставува централна точка за управување со целиот систем за креирање лабораториски распоред.

Главниот прозорец овозможува брз преглед на најважните информации поврзани со активниот распоред преку визуелни статистички картички.

Во почетниот приказ се прикажуваат:

* Вкупен број на внесени предмети
* Број на достапни лаборатории
* Број на креирани временски термини
* Активен семестар со кој моментално работи корисникот

Преку овој панел корисникот започнува со целокупниот процес на креирање распоред, од дефинирање на академскиот период, внесување предмети и лаборатории, па сè до финално распоредување и извоз на податоците.

---

# 2.2 Креирање и управување со семестри

Првиот чекор при користење на апликацијата е избор или креирање на семестар во кој ќе се организира лабораториската настава.

Системот овозможува работа со повеќе семестри, при што секој семестар претставува независна целина со сопствени податоци.

Достапни се:

* Семестар 1
* Семестар 2
* Семестар 3
* Семестар 4
* Семестар 5
* Семестар 6
* Семестар 7
* Семестар 8

Секој креиран семестар содржи сопствени:

* Предмети
* Лаборатории
* Распоред
* Доделени термини

Корисникот може да управува со семестрите преку:

* Додавање нов семестар
* Активирање на избран семестар
* Бришење на постоечки семестар

Со ова се овозможува одделно организирање на распоредите за различни академски периоди без мешање на податоците.

---

# 2.3 Управување со предмети

По изборот на активен семестар, корисникот може да додава предмети кои ќе бидат вклучени во креирањето на лабораторискиот распоред.

При додавање на предмет се внесуваат основни информации:

* Назив на предмет
* Тип на предмет
* Број на студенти кои го посетуваат предметот
* Фреквенција на одржување на лабораториските вежби

Предметите се визуелно разликуваат според нивниот тип:

📌 **Задолжителен предмет**

🟢 **Изборен предмет**

Фреквенцијата на одржување овозможува дефинирање колку често се реализира лабораториската вежба:

* Секоја недела
* На две недели
* На три недели

За секој предмет корисникот има можност да:

* Го промени бројот на студенти
* Ја промени фреквенцијата
* Ги зачува измените
* Го отстрани предметот од активниот семестар

---

# 2.4 Управување со лаборатории

За креирање реален распоред потребно е претходно да се дефинираат лабораториите кои се достапни за изведување на вежбите.

За секоја лабораторија се внесуваат:

* Назив на лабораторија
* Максимален број на студенти кои може да ги прими

Пример:

**Визуелно Програмирање **

Капацитет: 30 студенти

Капацитетот на лабораториите е важен параметар при организирање на распоредот бидејќи овозможува правилно распределување на студентите.

Апликацијата овозможува:

* Креирање нова лабораторија
* Промена на постоечки податоци
* Зачувување на измените
* Бришење лабораторија

---

# 2.5 Статистички приказ и анализа

Апликацијата содржи посебен дел за статистички преглед на распределбата на предметите.

За секој предмет се прикажуваат информации за:

* Вкупен број на студенти
* Број на веќе распределени студенти
* Број на преостанати студенти
* Процент на успешна распределба

Со помош на статистичкиот приказ корисникот може лесно да провери дали предметите се целосно организирани и дали постојат студенти кои немаат доделен термин.

---

# 2.6 Креирање на лабораториски распоред

Централната функционалност на апликацијата е интерактивното креирање на лабораториски распоред.

Распоредот е прикажан преку табеларен систем кој ги поврзува:

* Денот
* Временскиот термин
* Лабораторијата
* Предметот

Распоредот е организиран според работните денови:

* Понеделник
* Вторник
* Среда
* Четврток
* Петок

Достапни временски интервали:

* 08:00 - 09:30
* 09:30 - 11:00
* 11:00 - 12:30
* 12:30 - 14:00
* 14:00 - 15:30
* 15:30 - 17:00
* 17:00 - 18:30
* 18:30 - 20:00

Секој Slot во табелата претставува конкретна комбинација од:

* Ден
* Време
* Лабораторија

Во секој слот корисникот може да додели соодветен предмет, при што системот го зачувува распоредот и овозможува негово понатамошно користење.

---

# 2.7 Извоз на готов распоред

По завршување на креирањето на распоредот, корисникот може да го зачува и извезе финалниот резултат.

Апликацијата поддржува:

* Генерирање PDF документ
* Експорт во Excel датотека

Извезените документи го прикажуваат креираниот лабораториски распоред и овозможуваат негово:

* Споделување
* Печатење
* Дополнителна обработка

Со ова апликацијата обезбедува целосен процес — од креирање на семестар и внесување податоци, до автоматизирана организација и презентација на финалниот лабораториски распоред.

# 3. Техничка имплементација
## 3.1 Кориснички интерфејс (GUI)

Корисничкиот интерфејс е изработен со Razor Views во рамки на ASP.NET Core MVC архитектурата.

Главниот приказ овозможува целосно управување со лабораторискиот распоред преку интерактивен и визуелно организиран интерфејс.

Имплементирани се:

* Картички со статистички податоци
* Интерактивни табели
* Dropdown менија за избор на податоци
* Форми за додавање и измена на информации
* Responsive приказ
* Dark Mode функционалност

За подобро корисничко искуство користени се:

* Модерен дизајн со CSS градиенти
* Анимации при интеракција
* Јасно визуелно разликување на задолжителни и изборни предмети
* Toast пораки за успешни операции

# 3.2 Управување со податоци

Податоците во апликацијата се динамички преземени преку MVC моделот и се прикажуваат преку Razor синтакса.

Главните модели во системот се:

### Predmeti

Го претставува предметот кој се користи во лабораторискиот распоред.

Содржи информации за:

* Назив на предмет
* Тип на предмет
* Број на студенти
* Фреквенција на одржување

### Laboratorija

Го претставува просторот каде се одржуваат лабораториските вежби.

Содржи:

* Име на лабораторија
* Максимален капацитет

### Semester

Го дефинира академскиот период во кој се организира распоредот.

Секој семестар има сопствени:

* Предмети
* Лаборатории
* Термини

За зачувување на податоците се користи база на податоци преку Entity Framework Core.

Во базата се зачувуваат:

* Семестри
* Предмети
* Лаборатории
* Распореди
* Термини

Со користење на база на податоци овозможено е трајно зачувување на информациите и нивно повторно користење при следно стартување на апликацијата.
### Slot

Претставува конкретен термин во распоредот.

Секој слот е комбинација од:

* Ден
* Време
* Лабораторија
* Доделен предмет

# 3.3 Креирање на распоред

Главната функционалност на системот е креирање на лабораториски распоред.

Распоредот е организиран преку матрица која ги поврзува:

* Работните денови
* Временските интервали
* Достапните лаборатории

Системот овозможува:

* Доделување предмет во слободен термин
* Приказ на веќе доделени предмети
* Следење на број на распределени студенти

При избор на предмет од табелата, податоците автоматски се зачувуваат преку контролерот.

# 3.4 Статистичка анализа

Апликацијата содржи модул за следење на распределбата на студентите.

За секој предмет се пресметува:

* Вкупен број на студенти
* Доделени студенти
* Преостанати студенти
* Процент на реализација

Овие информации му овозможуваат на корисникот лесно да провери дали распоредот е целосно организиран.

# 3.5 Извоз на податоци

Имплементирана е можност за генерирање документи од креираниот распоред.

Поддржани формати:

## PDF

Овозможува:

* Преглед на финалниот распоред
* Печатење
* Архивирање

## Excel

Овозможува:

* Дополнителна обработка
* Анализа
* Лесно споделување на податоците

# 3.6 JavaScript функционалности

За подобрување на интерактивноста користени се JavaScript функции.

Имплементирани се:

* Вклучување и исклучување Dark Mode
* Автоматско додавање предмети преку Fetch API
* Зачувување на позицијата на скролирање
* Динамичка измена на лаборатории

# 3.7 Валидација и контрола

Апликацијата обезбедува контрола при внесување на податоци.

Проверки се прават при:

* Избор на предмет
* Додавање семестар
* Внесување капацитет на лабораторија
* Доделување термин

Со тоа се спречува внесување невалидни податоци и се одржува точноста на распоредот.

---

# English

# 1. Application Description

The **Lab Schedule** application is a web-based system designed for automated creation, organization, and management of laboratory exercise schedules.

The main purpose of the application is to provide a simple and efficient way to organize educational activities by connecting subjects, laboratories, students, and available time slots.

Using the application, the user can:

- Create and manage semesters
- Add and remove subjects
- Define the number of students
- Add laboratories with different capacities
- Create schedules based on days and time periods
- Monitor distribution statistics
- Save and export laboratory schedules in PDF and Excel formats

For better user experience, the application includes:

- Modern and clean design
- Responsive interface
- Dark Mode
- Interactive tables
- Automatic database storage
- PDF and Excel document generation

The application is developed using the ASP.NET Core MVC architecture, where the application logic, interface, and data are separated into different components.

---

# 2. Application Functionalities

## 2.1 Main Dashboard

When starting the application, the user enters the main dashboard, which represents the central place for managing the laboratory scheduling system.

The dashboard provides a quick overview of important information related to the current schedule through visual statistic cards.

The main page displays:

* Total number of subjects
* Number of available laboratories
* Number of created time slots
* Currently active semester

Through this dashboard, the user can manage the entire process of creating a schedule, from defining semesters and entering data to final schedule creation and exporting.

---

## 2.2 Semester Management

The first step in using the application is selecting or creating a semester where laboratory exercises will be organized.

The system supports multiple semesters, where each semester contains independent data.

Available semesters:

* Semester 1
* Semester 2
* Semester 3
* Semester 4
* Semester 5
* Semester 6
* Semester 7
* Semester 8

Each semester contains:

* Subjects
* Laboratories
* Schedule
* Assigned time slots

The user can:

* Add a new semester
* Select an active semester
* Delete an existing semester

---

## 2.3 Subject Management

After selecting a semester, the user can add subjects that will be included in the laboratory schedule.

Each subject contains:

* Subject name
* Subject type
* Number of students
* Laboratory exercise frequency

Subjects are visually separated into:

📌 **Mandatory Subject**

🟢 **Elective Subject**

The frequency option allows the user to define how often laboratory exercises are organized:

* Every week
* Every two weeks
* Every three weeks

For each subject the user can:

* Change student number
* Change frequency
* Save changes
* Remove the subject

---

## 2.4 Laboratory Management

Before creating a schedule, available laboratories need to be defined.

For each laboratory, the following information is entered:

* Laboratory name
* Maximum capacity

Example:

**Visual Programming**

Capacity: 30 students

The application allows:

* Creating new laboratories
* Editing existing data
* Saving changes
* Deleting laboratories

---

## 2.5 Statistics and Analysis

The application includes a statistics module for monitoring subject distribution.

For every subject, the system displays:

* Total number of students
* Assigned students
* Remaining students
* Completion percentage

This allows the user to easily check whether all students are successfully assigned to the schedule.

---

## 2.6 Creating Laboratory Schedule

The main functionality of the system is creating an interactive laboratory schedule.

The schedule connects:

* Day
* Time period
* Laboratory
* Subject

The schedule is organized by working days:

* Monday
* Tuesday
* Wednesday
* Thursday
* Friday

Available time periods:

* 08:00 - 09:30
* 09:30 - 11:00
* 11:00 - 12:30
* 12:30 - 14:00
* 14:00 - 15:30
* 15:30 - 17:00
* 17:00 - 18:30
* 18:30 - 20:00

Each slot represents:

* Day
* Time
* Laboratory
* Assigned subject

When selecting a subject, the data is automatically saved through the controller.

---

## 2.7 Exporting the Schedule

After completing the schedule, the user can export the final result.

Supported formats:

* PDF document
* Excel file

Exported documents allow:

* Sharing
* Printing
* Additional processing

---

# 3. Technical Implementation

## 3.1 User Interface (GUI)

The user interface is created using Razor Views within the ASP.NET Core MVC architecture.

Implemented features:

* Statistic cards
* Interactive tables
* Dropdown menus
* Forms for adding and editing data
* Responsive design
* Dark Mode

Additional improvements:

* Modern CSS gradients
* Interactive animations
* Visual difference between mandatory and elective subjects
* Toast notifications

---

## 3.2 Data Management

Application data is dynamically loaded through MVC models and displayed using Razor syntax.

Main system models:

### Predmeti

Represents a subject used in the laboratory schedule.

Contains:

* Subject name
* Subject type
* Number of students
* Exercise frequency

### Laboratorija

Represents a laboratory where exercises are held.

Contains:

* Laboratory name
* Maximum capacity

### Semester

Defines the academic period where the schedule is organized.

Each semester contains:

* Subjects
* Laboratories
* Time slots

The application uses a database through Entity Framework Core for permanent data storage.

Stored data includes:

* Semesters
* Subjects
* Laboratories
* Schedules
* Time slots

### Slot

Represents a specific schedule position.

Each slot contains:

* Day
* Time
* Laboratory
* Assigned subject

---

## 3.3 Schedule Creation

The main functionality of the system is creating laboratory schedules.

The schedule is organized as a matrix connecting:

* Working days
* Time intervals
* Available laboratories

The system allows:

* Assigning subjects to free slots
* Displaying assigned subjects
* Tracking student distribution

---

## 3.4 Statistics Module

The application calculates:

* Total students
* Assigned students
* Remaining students
* Completion percentage

This helps users verify whether the schedule is fully organized.

---

## 3.5 Data Export

The system supports document generation from created schedules.

Supported formats:

### PDF

Provides:

* Schedule preview
* Printing
* Archiving

### Excel

Provides:

* Data analysis
* Additional editing
* Easy sharing

---

## 3.6 JavaScript Features

JavaScript is used to improve application interaction.

Implemented features:

* Dark Mode switching
* Adding subjects using Fetch API
* Saving scroll position
* Dynamic laboratory editing

---

## 3.7 Validation and Control

The application provides validation during data input.

Checks are performed when:

* Selecting subjects
* Creating semesters
* Entering laboratory capacity
* Assigning schedule slots

This prevents invalid data and maintains schedule accuracy.
Last updated: 27 June 2026
