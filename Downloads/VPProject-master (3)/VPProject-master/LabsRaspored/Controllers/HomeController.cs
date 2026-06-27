using LabsRaspored.Models;
using LabsRaspored.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    static List<Semestar> semesters = new();
    static List<Predmeti> selectedSubjects = new();
    static List<Laboratorija> _labs = new List<Laboratorija>();

    // Конструктор каде што ја вбризгуваме базата и иницијализираме податоци
    public HomeController(ApplicationDbContext context)
    {
        _context = context;
        SeedSubjects();
    }


    public IActionResult ExportExcel(int semesterId)
    {
        var subjects = selectedSubjects
            .Where(x => x.SemesterId == semesterId)
            .ToList();

        var slotsForSemester = slots
            .Where(x => subjects.Any(s => s.Id == x.SubjectId))
            .ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Распоред");

        // HEADER
        worksheet.Cell(1, 1).Value = "Предмет";
        worksheet.Cell(1, 2).Value = "Студенти";
        worksheet.Cell(1, 3).Value = "Термин";
        worksheet.Cell(1, 4).Value = "Лабораторија";
        worksheet.Cell(1, 5).Value = "Доделени студенти";

        int row = 2;

        foreach (var subject in subjects)
        {
            var subjectSlots = slotsForSemester
                .Where(x => x.SubjectId == subject.Id)
                .ToList();

            if (!subjectSlots.Any())
            {
                worksheet.Cell(row, 1).Value = subject.Name;
                worksheet.Cell(row, 2).Value = subject.Students;
                worksheet.Cell(row, 3).Value = "-";
                worksheet.Cell(row, 4).Value = "-";
                worksheet.Cell(row, 5).Value = 0;
                row++;
            }
            else
            {
                foreach (var slot in subjectSlots)
                {
                    var lab = labs.FirstOrDefault(x => x.Id == slot.LabId);

                    worksheet.Cell(row, 1).Value = subject.Name;
                    worksheet.Cell(row, 2).Value = subject.Students;
                    worksheet.Cell(row, 3).Value = slot.Time;
                    worksheet.Cell(row, 4).Value = lab?.Name ?? "-";
                    worksheet.Cell(row, 5).Value = slot.AssignedStudents;

                    row++;
                }
            }
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "raspored.xlsx"
        );
    }

    // Методот што автоматски ќе ти ја наполни базата со предметите при првото пуштање
    private void SeedSubjects()
    {

        if (!_context.Semesters.Any())
        {
            for (int i = 1; i <= 8; i++)
            {
                _context.Semesters.Add(new Semestar
                {
                    Name = $"Семестар {i}"
                });
            }

            _context.SaveChanges();
        }

        // 2. Сега веќе постојат семестрите во базата, па ја проверуваме табелата за предмети
        if (_context.Subjects.Any())
        {
            return;
        }
        

        var initialSubjects = new List<Predmeti>
        {
            // ================= SEMESTAR 1 =================
            new Predmeti { Id = 1, Code = "F23L1W004", Name = "Спорт и здравје --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },
            new Predmeti { Id = 2, Code = "F23L1W005", Name = "Бизнис и менаџмент --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },
            new Predmeti { Id = 3, Code = "F23L1W007", Name = "Вовед во компјутерските науки --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },
            new Predmeti { Id = 4, Code = "F23L1W018", Name = "Професионални вештини --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },
            new Predmeti { Id = 5, Code = "F23L1W020", Name = "Структурно програмирање --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },
            new Predmeti { Id = 6, Code = "F23L2W003", Name = "Избрани теми од математика --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 1 },

            // ================= SEMESTAR 2 =================
            new Predmeti { Id = 7, Code = "F23L1S003", Name = "Архитектура и организација на компјутери --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 8, Code = "F23L1S016", Name = "Објектно-ориентирано програмирање --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 9, Code = "F23L1S023", Name = "Бизнис статистика --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 10, Code = "F23L1S146", Name = "Основи на Веб дизајн --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 2 },

            // IZBORNI L1
            new Predmeti { Id = 11, Code = "F23L1S052", Name = "Е-учење --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 12, Code = "F23L1S116", Name = "Компјутерски компоненти --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 13, Code = "F23L1S120", Name = "Креативни вештини за решавање проблеми --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 2 },
            new Predmeti { Id = 14, Code = "F23L1S066", Name = "Основи на сајбер безбедноста --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 2 },

            // ================= SEMESTAR 3 =================
            new Predmeti { Id = 15, Code = "F23L2W014", Name = "Компјутерски мрежи и безбедност --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 16, Code = "F23L2W100", Name = "Економија за ИКТ инженери --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 17, Code = "F23L2W201", Name = "Примена на алгоритми и податочни структури --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 3 },

            // IZBORNI L2W
            new Predmeti { Id = 18, Code = "F23L2W006", Name = "Веројатност и статистика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 19, Code = "F23L2W055", Name = "Мултимедијални технологии --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 20, Code = "F23L2W104", Name = "Инженерска математика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 21, Code = "F23L2W109", Name = "Интернет програмирање на клиентска страна --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 22, Code = "F23L2W167", Name = "Шаблони за дизајн на кориснички интерфејси --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 23, Code = "F23L2W067", Name = "Основи на теоријата на информации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 24, Code = "F23L2W096", Name = "Дигитизација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 25, Code = "F23L2W147", Name = "Основи на комуникациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },
            new Predmeti { Id = 26, Code = "F23L2W165", Name = "Управување со техничка поддршка --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 3 },

            // ================= SEMESTAR 4 =================
            new Predmeti { Id = 27, Code = "F23L2S026", Name = "Маркетинг --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 28, Code = "F23L2S017", Name = "Оперативни системи --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 29, Code = "F23L2S029", Name = "Софтверско инженерство --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 30, Code = "F23L3S100", Name = "Деловна пракса --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 4 },

            // IZBORNI L2S
            new Predmeti { Id = 31, Code = "F23L2S015", Name = "Објектно ориентирана анализа и дизајн --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 32, Code = "F23L2S002", Name = "Анализа на софтверските барања --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 33, Code = "F23L2S030", Name = "Вештачка интелигенција --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 34, Code = "F23L2S061", Name = "Безжични и мобилни системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 35, Code = "F23L2S110", Name = "Интернет технологии --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 36, Code = "F23L2S114", Name = "Компјутерска графика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 37, Code = "F23L2S042", Name = "Електрични кола --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 38, Code = "F23L2S051", Name = "Информатичко размислување во образованието --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 39, Code = "F23L2S082", Name = "Визуелно програмирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 40, Code = "F23L2S084", Name = "Вовед во екоинформатиката --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 41, Code = "F23L2S090", Name = "Вовед во случајни процеси --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 42, Code = "F23L2S095", Name = "Дигитално процесирање на слика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 43, Code = "F23L2S097", Name = "Дизајн на алгоритми --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 44, Code = "F23L2S099", Name = "Е-влада --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 45, Code = "F23L2S119", Name = "Концепти на информатичко општество --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 46, Code = "F23L2S124", Name = "Медиуми и комуникации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },
            new Predmeti { Id = 47, Code = "F23L2S164", Name = "Теорија на информации со дигитални комуникации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 4 },

            // ================= SEMESTAR 5 =================
            new Predmeti { Id = 48, Code = "F23L3W004", Name = "Бази на податоци --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 49, Code = "F23L3W008", Name = "Вовед во науката за податоци --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 50, Code = "F23L3W024", Name = "Веб програмирање --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 51, Code = "F23L3W136", Name = "Напреден веб дизајн --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 5 },

            // IZBORNI F23L2W (Semester 5)
            new Predmeti { Id = 52, Code = "F23L2W006", Name = "Веројатност и статистика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 53, Code = "F23L2W055", Name = "Мултимедијални технологии --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 54, Code = "F23L2W104", Name = "Инженерска математика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 55, Code = "F23L2W109", Name = "Internet програмирање на клиентска страна --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 56, Code = "F23L2W167", Name = "Шаблони за дизајн на кориснички интерфејси --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 57, Code = "F23L2W067", Name = "Основи на теоријата на информации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 58, Code = "F23L2W096", Name = "Дигитизација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 59, Code = "F23L2W147", Name = "Основи на комуникациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 60, Code = "F23L2W165", Name = "Управување со техничка поддршка --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },

            // IZBORNI F23L3W (Semester 5)
            new Predmeti { Id = 61, Code = "F23L3W001", Name = "Математика 3 --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 62, Code = "F23L3W009", Name = "Дизајн и архитектура на софтвер --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 63, Code = "F23L3W035", Name = "Линеарна алгебра и примени --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 64, Code = "F23L3W037", Name = "Паралелно и дистрибуирано процесирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 65, Code = "F23L3W043", Name = "Информациска безбедност --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 66, Code = "F23L3W044", Name = "Компјутерска електроника --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 67, Code = "F23L3W050", Name = "Дизајн на образовен софтвер --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 68, Code = "F23L3W053", Name = "Компјутерска етика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 69, Code = "F23L3W056", Name = "Персонализирано учење --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 70, Code = "F23L3W060", Name = "Администрација на системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 71, Code = "F23L3W065", Name = "Сајбер безбедност --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 72, Code = "F23L3W081", Name = "Визуелизација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 73, Code = "F23L3W134", Name = "Мултимедиски мрежи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 74, Code = "F23L3W140", Name = "Напредно програмирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 75, Code = "F23L3W142", Name = "Обработка на природните јазици --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 76, Code = "F23L3W148", Name = "Основи на роботиката --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 77, Code = "F23L3W158", Name = "Современи компјутерски архитектури --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },
            new Predmeti { Id = 78, Code = "F23L3W161", Name = "Теорија на графови и социјални мрежи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 5 },

            // ================= SEMESTAR 6 =================
            new Predmeti { Id = 79, Code = "F23L3S010", Name = "Дизајн на интеракцијата човек-компјутер --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 80, Code = "F23L3S025", Name = "Електронска и мобилна трговија --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 6 },

            // IZBORNI F23L3S (Semester 6)
            new Predmeti { Id = 81, Code = "F23L3S012", Name = "Интегрирани системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 82, Code = "F23L3S036", Name = "Машинско учење --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 83, Code = "F23L3S039", Name = "Основи на теоријата на компјутерските науки --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 84, Code = "F23L3S040", Name = "Вградливи микропроцесорски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 85, Code = "F23L3S047", Name = "Процесирање на сигналите --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 86, Code = "F23L3S057", Name = "Работа со надарени ученици --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 87, Code = "F23L3S059", Name = "Администрација на мрежи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 88, Code = "F23L3S062", Name = "Виртуелизација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 89, Code = "F23L3S071", Name = "Психологија на училишна возраст --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 90, Code = "F23L3S073", Name = "Агентно-базирани системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 91, Code = "F23L3S087", Name = "Вовед во мрежна наука --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 92, Code = "F23L3S091", Name = "Географски информациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 93, Code = "F23L3S093", Name = "Дигитална форензика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 94, Code = "F23L3S094", Name = "Дигитални библиотеки --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 95, Code = "F23L3S113", Name = "Компјутерска анимација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 96, Code = "F23L3S115", Name = "Компјутерски звук, музика и говор --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 97, Code = "F23L3S118", Name = "Континуирана интеграција и испорака --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 98, Code = "F23L3S122", Name = "Криптографија --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 99, Code = "F23L3S125", Name = "Мерење и анализа на сообраќај --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 100, Code = "F23L3S135", Name = "Мултимедиски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 101, Code = "F23L3S138", Name = "Напредни бази на податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 102, Code = "F23L3S149", Name = "Паралелно програмирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 103, Code = "F23L3S150", Name = "Податочно рударење --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 104, Code = "F23L3S153", Name = "Вештачка интелигенција за игри --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 105, Code = "F23L3S155", Name = "Сервисно ориентирани архитектури --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 106, Code = "F23L3S157", Name = "Складови на податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 107, Code = "F23L3S159", Name = "Софтверски дефинирана безбедност --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 108, Code = "F23L3S163", Name = "Автоматизирање на ML процеси --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },
            new Predmeti { Id = 109, Code = "F23L3S166", Name = "Учење на далечина --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 6 },

            // ================= SEMESTAR 7 =================
            new Predmeti { Id = 110, Code = "F23L3W021", Name = "Тимски проект --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 111, Code = "F23L3W027", Name = "Менаџмент информациски системи --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 112, Code = "F23L3W033", Name = "Тестирање на софтвер --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 7 },

            // IZBORNI F23L3W (Semester 7)
            new Predmeti { Id = 113, Code = "F23L3W038", Name = "Програмски парадигми --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 114, Code = "F23L3W048", Name = "Софтвер за вградливи системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 115, Code = "F23L3W064", Name = "Дистрибуирани системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 116, Code = "F23L3W068", Name = "Пресметување во облак --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 117, Code = "F23L3W072", Name = "Автономна роботика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 118, Code = "F23L3W074", Name = "Администрација на бази податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 119, Code = "F23L3W075", Name = "Анализа и дизајн на информациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 120, Code = "F23L3W076", Name = "Анализа на временски серии --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 121, Code = "F23L3W079", Name = "Веб базирани системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 122, Code = "F23L3W085", Name = "Биоинформатика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 123, Code = "F23L3W088", Name = "Паметни градови --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 124, Code = "F23L3W089", Name = "Препознавање на облици --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 125, Code = "F23L3W092", Name = "Дигитална постпродукција --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 126, Code = "F23L3W098", Name = "Дистрибуирано складирање на податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 127, Code = "F23L3W103", Name = "Имплементација на системи со отворен код --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 128, Code = "F23L3W105", Name = "Иновации во ИКТ --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 129, Code = "F23L3W108", Name = "Интернет на нештата --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 130, Code = "F23L3W117", Name = "Компјутерски поддржано производство --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 131, Code = "F23L3W121", Name = "Блокчекери и криптовалути --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 132, Code = "F23L3W123", Name = "Машинска визија --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 133, Code = "F23L3W126", Name = "Методологија на истражување во ИКТ --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 134, Code = "F23L3W128", Name = "Мобилни информациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 135, Code = "F23L3W129", Name = "Мобилни платформи и програмирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 136, Code = "F23L3W133", Name = "Мрежна и мобилна форензика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 137, Code = "F23L3W137", Name = "Напредна интеракција човек-компјутер --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 138, Code = "F23L3W145", Name = "Оптички мрежи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 139, Code = "F23L3W152", Name = "Програмирање на видео игри --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 140, Code = "F23L3W154", Name = "Рударење на масивни податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 141, Code = "F23L3W156", Name = "Системи за поддршка при одлучување --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 142, Code = "F23L3W162", Name = "Квантно пресметување --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },
            new Predmeti { Id = 143, Code = "F23L3W200", Name = "Сензорски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 7 },

            // ================= SEMESTAR 8 =================
            new Predmeti { Id = 144, Code = "F23L3S022", Name = "Управување со ИКТ проекти --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 145, Code = "F23L3S028", Name = "Претприемништво --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 146, Code = "F23L3S168", Name = "Дипломска работа --(ЗАДОЛЖИТЕЛЕН ПРЕДМЕТ)", SemesterId = 8 },

            // IZBORNI F23L3S (Semester 8)
            new Predmeti { Id = 147, Code = "F23L3S054", Name = "Методика на информатиката --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 148, Code = "F23L3S063", Name = "Дизајн на компјутерски мрежи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 149, Code = "F23L3S069", Name = "Адаптивни и интерактивни веб информациски системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 150, Code = "F23L3S070", Name = "Македонски јазик --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 151, Code = "F23L3S078", Name = "Биолошки инспирирано пресметување --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 152, Code = "F23L3S080", Name = "Веб пребарувачки системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 153, Code = "F23L3S083", Name = "Виртуелна реалност --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 154, Code = "F23L3S086", Name = "Вовед во когнитивни науки --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 155, Code = "F23L3S101", Name = "Етичко хакирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 156, Code = "F23L3S102", Name = "ИКТ за развој --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 157, Code = "F23L3S106", Name = "Откривање знаење со длабоко учење --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 158, Code = "F23L3S107", Name = "Интелигентни системи --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 159, Code = "F23L3S111", Name = "Инфраструктурно програмирање --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 160, Code = "F23L3S112", Name = "Програмски јазици и компајлери --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 161, Code = "F23L3S127", Name = "Мобилни апликации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 162, Code = "F23L3S130", Name = "Моделирање и менаџирање на бизнис процеси --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 163, Code = "F23L3S131", Name = "Моделирање и симулација --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 164, Code = "F23L3S132", Name = "Модерни трендови во роботика --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 165, Code = "F23L3S139", Name = "Web3 апликации --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 },
            new Predmeti { Id = 166, Code = "F23L3S141", Name = "Неструктурирани бази на податоци --(ИЗБОРЕН ПРЕДМЕТ)", SemesterId = 8 }
        };

        //_context.Subjects.AddRange(initialSubjects);
        //_context.SaveChanges();
        using (var transaction = _context.Database.BeginTransaction())
        {
            _context.Subjects.AddRange(initialSubjects);

            // Му кажуваме на SQL Server привремено да го исклучи Auto-increment за оваа табела
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Subjects ON");

            _context.SaveChanges();

            // Го враќаме назад Auto-increment за да не се расипе ништо во иднина
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Subjects OFF");

            transaction.Commit();
        }
    }

    static List<Laboratorija> labs = new();
    static List<Slot> slots = new();

    // ================= INDEX =================
    public IActionResult Index(int? semesterId)
    {
        ViewBag.Semesters = semesters;
        ViewBag.SelectedSemesterId = semesterId;

        ViewBag.SelectedSemester = semesters
            .FirstOrDefault(x => x.Id == semesterId);

        ViewBag.AllSubjects = semesterId != null
            ? _context.Subjects.Where(x => x.SemesterId == semesterId).ToList()
            : new List<Predmeti>();

        ViewBag.Labs = labs;
        ViewBag.Slots = slots;

        // 🔥 FIX HERE
        var subjects = semesterId == null
            ? selectedSubjects
            : selectedSubjects.Where(x => x.SemesterId == semesterId).ToList();

        return View(subjects);
    }

    // ================= ADD SEMESTER =================
    [HttpGet]
    public IActionResult AddSemester(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            TempData["Message"] = "❌ Внеси име";
            return RedirectToAction("Index");
        }

        int semesterNumber = int.Parse(name.Replace("Семестар ", ""));

        semesters.Add(new Semestar
        {
            Id = semesterNumber,
            Name = name
        });

        return RedirectToAction("Index", new { semesterId = semesterNumber });
    }

    // ================= DELETE SEMESTER =================
    [HttpPost]
    public IActionResult DeleteSemester(int id)
    {
        semesters.RemoveAll(x => x.Id == id);

        var subjectIds = selectedSubjects
            .Where(x => x.SemesterId == id)
            .Select(x => x.Id)
            .ToList();

        selectedSubjects.RemoveAll(x => x.SemesterId == id);

        labs.Clear();
        slots.RemoveAll(x => subjectIds.Contains(x.SubjectId));

        return RedirectToAction("Index");
    }

    // ================= ADD SUBJECT (FIXED ID) =================
    [HttpPost]
    [HttpPost]
    [HttpPost]
    public IActionResult AddSubject(int subjectId, int semesterId, int students)
    {
        // Проверка дали веќе е додаден во избрани за тој семестар
        if (selectedSubjects.Any(x => x.Id == subjectId && x.SemesterId == semesterId))
        {
            TempData["Message"] = "❌ Предметот е веќе додаден во овој семестар.";
            return RedirectToAction("Index", new { semesterId });
        }

        var dbSubject = _context.Subjects.Find(subjectId);
        if (dbSubject != null)
        {
            selectedSubjects.Add(new Predmeti
            {
                Id = dbSubject.Id,
                Name = dbSubject.Name,
                Students = students,
                SemesterId = semesterId
            });
        }
        return RedirectToAction("Index", new { semesterId });
    }

    // ================= UPDATE SUBJECT =================
    [HttpPost]
    public IActionResult UpdateSubject(int id, int semesterId, int students, string frequency)
    {
        var subject = selectedSubjects
            .FirstOrDefault(x => x.Id == id && x.SemesterId == semesterId);

        if (subject == null)
        {
            TempData["Message"] = "❌ Не постои";
            return RedirectToAction("Index", new { semesterId });
        }

        subject.Students = students;
        subject.Frequency = frequency;

        return RedirectToAction("Index", new { semesterId });
    }

    // ================= DELETE SUBJECT (FIXED SAFELY) =================
    [HttpPost]
    public IActionResult DeleteSubject(int id, int semesterId)
    {
        selectedSubjects.RemoveAll(x => x.Id == id && x.SemesterId == semesterId);
        slots.RemoveAll(x => x.SubjectId == id);

        return RedirectToAction("Index", new { semesterId });
    }

    // ================= ADD LAB =================
    [HttpPost]
    public IActionResult AddLab(string name, int capacity, int semesterId)
    {
        labs.Add(new Laboratorija
        {
            Id = labs.Count == 0 ? 1 : labs.Max(x => x.Id) + 1,
            Name = name,
            Capacity = capacity
        });

        return RedirectToAction("Index", new { semesterId });
    }
    // ================= ASSIGN SLOT =================
    [HttpPost]
    // ================= ASSIGN SLOT (ПОПРАВЕН ЗА DROP-DOWN МАТРИЦА) =================
    [HttpPost]
    public IActionResult AssignSlot(string time, int labId, int? subjectId, int semesterId)
    {
        // 1. ПРОВЕРКА: Дали корисникот избрал „Избери предмет“ (сака да го испразни терминот)
        if (subjectId == null || subjectId == 0)
        {
            // Го наоѓаме зафатениот термин за ова време и оваа лабораторија и го бришеме
            slots.RemoveAll(x => x.Time == time && x.LabId == labId);
            TempData["Message"] = "🗑 Терминот е успешно ослободен.";
            return RedirectToAction("Index", new { semesterId });
        }

        // 2. АКО Е ИЗБРАН ПРЕДМЕТ, продолжуваме со нормална алокација
        var subject = selectedSubjects
            .FirstOrDefault(x => x.Id == subjectId && x.SemesterId == semesterId);

        var lab = labs.FirstOrDefault(x => x.Id == labId);

        if (subject == null || lab == null)
        {
            TempData["Message"] = "❌ Предметот или лабораторијата не се пронајдени.";
            return RedirectToAction("Index", new { semesterId });
        }

        // 3. ПРЕД ДА ПРЕСМЕТУВАМЕ КАПАЦИТЕТ: Го тргаме претходниот запис за ОВАА ќелија ако постоел, 
        // за да дозволиме чиста замена на предмет или рекалкулација
        slots.RemoveAll(x => x.Time == time && x.LabId == labId);

        // 4. Колку студенти веќе се распределени за овој предмет во ДРУГИТЕ термини
        var totalAssigned = slots
            .Where(x => x.SubjectId == subject.Id)
            .Sum(x => x.AssignedStudents);

        var remainingStudents = subject.Students - totalAssigned;

        // Ако нема останати студенти за овој предмет
        if (remainingStudents <= 0)
        {
            TempData["Message"] = "❌ Сите студенти од овој предмет се веќе распределени во други термини.";
            return RedirectToAction("Index", new { semesterId });
        }

        // 5. Проверка за слободен капацитет на лабораторијата (во овој момент е чиста бидејќи ја исчистивме во чекор 3)
        var usedCapacity = slots
            .Where(x => x.LabId == labId && x.Time == time)
            .Sum(x => x.AssignedStudents);

        var availableCapacity = lab.Capacity - usedCapacity;

        if (availableCapacity <= 0)
        {
            TempData["Message"] = "❌ Лабораторијата е полна за овој термин.";
            return RedirectToAction("Index", new { semesterId });
        }

        // 6. Пресметка колку реално доделуваме
        int toAssign = Math.Min(remainingStudents, availableCapacity);

        // 7. Додавање на новиот чист слот
        slots.Add(new Slot
        {
            Id = slots.Count == 0 ? 1 : slots.Max(x => x.Id) + 1,
            Time = time,
            LabId = labId,
            SubjectId = subject.Id,
            AssignedStudents = toAssign
        });

        TempData["Message"] = $"✅ Успешно распределени {toAssign} студенти за {subject.Name}.";
        return RedirectToAction("Index", new { semesterId });
    }

    [HttpPost]
    public IActionResult UpdateLab(int id, string name, int capacity, int semesterId)
    {
        var lab = labs.FirstOrDefault(x => x.Id == id);

        if (lab != null)
        {
            lab.Name = name;
            lab.Capacity = capacity;
        }

        return RedirectToAction("Index", new { semesterId });
    }

    [HttpPost]
    public IActionResult DeleteLab(int id, int semesterId)
    {
        var lab = labs.FirstOrDefault(x => x.Id == id);

        if (lab != null)
        {
            labs.Remove(lab);
        }

        return RedirectToAction("Index", new { semesterId });
    }
    public IActionResult ExportPdf(int semesterId)
    {
        var subjects = selectedSubjects
            .Where(x => x.SemesterId == semesterId)
            .ToList();

        var slotsForSemester = slots
            .Where(x => subjects.Any(s => s.Id == x.SubjectId))
            .ToList();

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);

                page.Content().Column(col =>
                {
                    col.Item().Text("📘 Распоред").FontSize(20).Bold();

                    col.Item().PaddingBottom(10);

                    foreach (var subject in subjects)
                    {
                        col.Item().Text($"• {subject.Name} - {subject.Students} студенти")
                            .FontSize(12)
                            .Bold();

                        var subjectSlots = slotsForSemester
                            .Where(x => x.SubjectId == subject.Id)
                            .ToList();

                        if (!subjectSlots.Any())
                        {
                            col.Item().Text("   (нема термин)").FontSize(10);
                        }
                        else
                        {
                            foreach (var slot in subjectSlots)
                            {
                                var lab = labs.FirstOrDefault(x => x.Id == slot.LabId);

                                col.Item().Text(
                                    $"   🕒 {slot.Time} | 🧪 {lab?.Name} | 👥 {slot.AssignedStudents}"
                                ).FontSize(10);
                            }
                        }

                        col.Item().PaddingBottom(8);
                    }
                });
            });
        });

        var stream = new MemoryStream();
        pdf.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "raspored.pdf");
    }
}


