/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Bitrix24
{
    /// <summary>
    /// Contains constants specific to the <see cref="Bitrix24AuthenticationHandler"/>.
    /// </summary>
    public static class Bitrix24AuthenticationConstants
    {
        public static class Claims
        {
            public const string IsActive = "active";
            public const string MiddleName = "second_name";
            public const string City = "personal_city";
            public const string Photo = "personal_photo";
            public const string ICQ = "personal_icq";
            public const string Fax = "personal_fax";
            public const string Pager = "personal_pager";
            public const string Company = "work_company";
            public const string Position = "work_position";
            public const string Profession = "personal_profession";
            public const string UfDepartment = "uf_department";
            public const string UfInterests = "uf_interests";
            public const string UfSkills = "uf_skills";
            public const string UfWebSites = "uf_web_sites";
            public const string UfXing = "uf_xing";
            public const string UfLinkedin = "uf_linkedin";
            public const string UfFacebook = "uf_facebook";
            public const string UfTwitter = "uf_twitter";
            public const string UfSkype = "uf_skype";
            public const string UfDistrict = "uf_district";
            public const string UfPhoneInner = "uf_phone_inner";
        }

        internal static class UserFields
        {
            public const string Id = "ID";
            public const string Active = "ACTIVE";
            public const string Email = "EMAIL";
            public const string Name = "NAME";
            public const string LastName = "LAST_NAME";
            public const string SecondName = "SECOND_NAME";
            public const string PersonalGender = "PERSONAL_GENDER";
            public const string PersonalProfession = "PERSONAL_PROFESSION";
            public const string PersonalWww = "PERSONAL_WWW";
            public const string PersonalBirthday = "PERSONAL_BIRTHDAY";
            public const string PersonalPhoto = "PERSONAL_PHOTO";
            public const string PersonalIcq = "PERSONAL_ICQ";
            public const string PersonalPhone = "PERSONAL_PHONE";
            public const string PersonalFax = "PERSONAL_FAX";
            public const string PersonalMobile = "PERSONAL_MOBILE";
            public const string PersonalPager = "PERSONAL_PAGER";
            public const string PersonalStreet = "PERSONAL_STREET";
            public const string PersonalCity = "PERSONAL_CITY";
            public const string PersonalState = "PERSONAL_STATE";
            public const string PersonalZip = "PERSONAL_ZIP";
            public const string PersonalCountry = "PERSONAL_COUNTRY";
            public const string WorkCompany = "WORK_COMPANY";
            public const string WorkPosition = "WORK_POSITION";
            public const string UfDepartment = "UF_DEPARTMENT";
            public const string UfInterests = "UF_INTERESTS";
            public const string UfSkills = "UF_SKILLS";
            public const string UfWebSites = "UF_WEB_SITES";
            public const string UfXing = "UF_XING";
            public const string UfLinkedin = "UF_LINKEDIN";
            public const string UfFacebook = "UF_FACEBOOK";
            public const string UfTwitter = "UF_TWITTER";
            public const string UfSkype = "UF_SKYPE";
            public const string UfDistrict = "UF_DISTRICT";
            public const string UfPhoneInner = "UF_PHONE_INNER";
        }

        public static class Scopes
        {
            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/bizproc/index.php" target="_blank">Бизнес-процессы</a>
            /// </summary>
            public const string BizProc = "bizproc";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/calendar/index.php">Calendar</a>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/calendar/index.php" target="_blank">Календарь</a>
            /// </summary>
            public const string Calendar = "calendar";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithsound.php" target="_blank">voximplant.infocall.startwithsound</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithtext.php" target="_blank">voximplant.infocall.startwithtext</a>
            /// </summary>
            public const string Call = "call";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/catalog/index.php" target="_blank">Торговый каталог</a>
            /// </summary>
            public const string Catalog = "catalog";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php#contact_center" target="_blank">Контакт-центр</a>
            /// </summary>
            public const string ContactCenter = "contact_center";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/crm/index.php" title="" target="_blank">CRM (English)</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/crm/index.php" title="" target="_blank">CRM (Русский)</a>
            /// </summary>
            public const string Crm = "crm";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/departments/index.php" title="" target="_blank">Departments</a>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/departments/index.php" title="" target="_blank">Структура компании</a>
            /// </summary>
            public const string Department = "department";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/disk/index.php" title="" target="_blank">Drive</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/disk/index.php" title="" target="_blank">Диск</a>
            /// </summary>
            public const string Disk = "disk";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/documentgenerator/index.php" title="" target="_blank">Генератор документов</a>
            /// </summary>
            public const string DocumentGenerator = "documentgenerator";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/entity/index.php" title="" target="_blank">Data Storage</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/entity/index.php" title="" target="_blank">Хранилище данных</a>
            /// </summary>
            public const string Entity = "entity";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/faceid/index.php" title="" target="_blank">Распознавание лиц</a>
            /// </summary>
            public const string FaceId = "faceid";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/im/index.php" title="" target="_blank">Notifications</a>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/im/index.php" title="" target="_blank">Чат и уведомления</a>
            /// </summary>
            public const string Im = "im";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/imbot/index.php" title="" target="_blank">Chat bots</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/imbot/index.php" title="" target="_blank">Создание и управление Чат-ботами</a>
            /// </summary>
            public const string ImBot = "imbot";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/learning/course/index.php?COURSE_ID=93&amp;LESSON_ID=8025" title="" target="_blank">Открытые линии</a>
            /// </summary>
            public const string ImOpenLines = "imopenlines";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php#intranet" target="_blank">Интранет</a>
            /// </summary>
            public const string Intranet = "intranet";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/landing/index.php" title="" target="_blank">Сайты</a>
            /// </summary>
            public const string Landing = "landing";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/lists/index.php" title="" target="_blank">Lists</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/lists/index.php" title="" target="_blank">Списки</a>
            /// </summary>
            public const string Lists = "lists";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/log/index.php" title="" target="_blank">Activity Stream</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/log/index.php" title="" target="_blank">Живая лента</a>
            /// </summary>
            public const string Log = "log";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/mailservice/index.php" title="" target="_blank">Почтовые сервисы</a>
            /// </summary>
            public const string MailService = "mailservice";

            /// <summary>
            /// https://training.bitrix24.com/rest_help/message_service/index.php
            /// https://dev.1c-bitrix.ru/rest_help/messageservice/index.php
            /// </summary>
            public const string MessageService = "messageservice";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/mobile/index.php" title="" target="_blank">Мобильное приложение</a>
            /// </summary>
            public const string Mobile = "mobile";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/paysystem/index.php" title="" target="_blank">payment systems</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/paysystem/index.php" title="" target="_blank">Платёжные системы</a>
            /// </summary>
            public const string PaySystem = "pay_system";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/application_embedding/index.php" title="" target="_blank">Application embedding</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php" title="" target="_blank">Встраивание приложений</a>
            /// </summary>
            public const string Placement = "placement";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/pull_push/index.php" title="" target="_blank">Pull&amp;Push</a>
            /// </summary>
            public const string Pull = "pull";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/rpa/index.php" title="" target="_blank">Роботизация бизнеса</a>
            /// </summary>
            public const string Rpa = "rpa";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/sale/index.php" title="" target="_blank">Интернет-магазин</a>
            /// </summary>
            public const string Sale = "sale";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/sonet_group/index.php" title="" target="_blank">Workgroups</a>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/socialnetwork/sonet_group/index.php" title="" target="_blank">Рабочие группы</a>
            /// </summary>
            public const string SonetGroup = "sonet_group";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/tasks/index.php" title="" target="_blank">Tasks</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/tasks/index.php" title="" target="_blank">Задачи</a>
            /// </summary>
            public const string Task = "task";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/scope_telephony/index.php">Telephony</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/scope_telephony/index.php" target="_blank">Телефония</a>
            /// </summary>
            public const string Telephony = "telephony";

            /// <summary>
            /// <a href="https://dev.1c-bitrix.ru/rest_help/timeman/index.php" title="" target="_blank">Учет рабочего времени</a>
            /// </summary>
            public const string Timeman = "timeman";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/users/index.php" title="" target="_blank">Users</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/users/index.php" title="" target="_blank">Пользователи</a>
            /// </summary>
            public const string User = "user";

            /// <summary>
            /// <a href="https://training.bitrix24.com/rest_help/userconsent/index.php" title="" target="_blank">User agreement</a>
            /// <a href = "https://dev.1c-bitrix.ru/rest_help/userconsent/index.php" title="" target="_blank">Работа с соглашениями</a>
            /// </summary>
            public const string UserConsent = "userconsent";
        }
    }
}
