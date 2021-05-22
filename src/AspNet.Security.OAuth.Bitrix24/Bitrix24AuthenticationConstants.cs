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
            public const string Icq = "personal_icq";
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

        public static class UserFields
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
            /// Workflows (<a href="https://training.bitrix24.com/rest_help/workflows/index.php">documentation</a>)
            /// Бизнес-процессы (<a href="https://dev.1c-bitrix.ru/rest_help/bizproc/index.php">документация</a>)
            /// </summary>
            public const string BizProc = "bizproc";

            /// <summary>
            /// Calendar (<a href="https://training.bitrix24.com/rest_help/calendar/index.php">documentation</a>)<br/>
            /// Календарь (<a href="https://dev.1c-bitrix.ru/rest_help/calendar/index.php">документация</a>)
            /// </summary>
            public const string Calendar = "calendar";

            /// <summary>
            /// Calls, required for <see cref="Telephony"/> when using methods <a href="https://training.bitrix24.com/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithsound.php">voximplant.infocall.startwithsound</a>, <a href="https://training.bitrix24.com/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithtext.php">voximplant.infocall.startwithtext</a>)<br/>
            /// Звонки, требуется для <see cref="Telephony"/> при использовании методов <a href="https://dev.1c-bitrix.ru/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithsound.php">voximplant.infocall.startwithsound</a>, <a href="https://dev.1c-bitrix.ru/rest_help/scope_telephony/voximplant/voximplant_infocall_startwithtext.php">voximplant.infocall.startwithtext</a>)
            /// </summary>
            public const string Call = "call";

            /// <summary>
            /// Commercial Catalog (<a href="https://training.bitrix24.com/rest_help/catalog/index.php">documentation</a>)<br/>
            /// Торговый каталог (<a href="https://dev.1c-bitrix.ru/rest_help/catalog/index.php">документация</a>)
            /// </summary>
            public const string Catalog = "catalog";

            /// <summary>
            /// Contact-center (<a href="https://training.bitrix24.com/rest_help/application_embedding/index.php#contact_center">documentation</a>)<br/>
            /// Контакт-центр (<a href="https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php#contact_center">документация</a>)
            /// </summary>
            public const string ContactCenter = "contact_center";

            /// <summary>
            /// CRM (English <a href="https://training.bitrix24.com/rest_help/crm/index.php">documentation</a>)<br/>
            /// CRM (на русском <a href="https://dev.1c-bitrix.ru/rest_help/crm/index.php">документация</a>)
            /// </summary>
            public const string Crm = "crm";

            /// <summary>
            /// Departments (<a href="https://training.bitrix24.com/rest_help/departments/index.php">documentation</a>)<br/>
            /// Структура компании (<a href="https://dev.1c-bitrix.ru/rest_help/departments/index.php">документация</a>)
            /// </summary>
            public const string Department = "department";

            /// <summary>
            /// Drive (<a href="https://training.bitrix24.com/rest_help/disk/index.php">documentation</a>)<br/>
            /// Диск (<a href="https://dev.1c-bitrix.ru/rest_help/disk/index.php">документация</a>)
            /// </summary>
            public const string Disk = "disk";

            /// <summary>
            /// Document Generator (<a href="https://training.bitrix24.com/rest_help/documentgenerator/index.php">documentation</a>)<br/>
            /// Генератор документов (<a href="https://dev.1c-bitrix.ru/rest_help/documentgenerator/index.php">документация</a>)
            /// </summary>
            public const string DocumentGenerator = "documentgenerator";

            /// <summary>
            /// Data Storage (<a href="https://training.bitrix24.com/rest_help/entity/index.php">documentation</a>)<br/>
            /// Хранилище данных (<a href="https://dev.1c-bitrix.ru/rest_help/entity/index.php">документация</a>)
            /// </summary>
            public const string Entity = "entity";

            /// <summary>
            /// Face Identification (no documentation in English)<br/>
            /// Распознавание лиц (<a href="https://dev.1c-bitrix.ru/rest_help/faceid/index.php">документация</a>)
            /// </summary>
            public const string FaceId = "faceid";

            /// <summary>
            /// Notifications (<a href="https://training.bitrix24.com/rest_help/im/index.php">documentation</a>)<br/>
            /// Чат и уведомления (<a href="https://dev.1c-bitrix.ru/rest_help/im/index.php">документация</a>)
            /// </summary>
            public const string Im = "im";

            /// <summary>
            /// Chat Bots (<a href="https://training.bitrix24.com/rest_help/imbot/index.php">documentation</a>)<br/>
            /// Чат-боты, сообщения и Открытые линии (<a href="https://dev.1c-bitrix.ru/rest_help/imbot/index.php">документация</a>)
            /// </summary>
            public const string ImBot = "imbot";

            /// <summary>
            /// Open channels (<a href="https://training.bitrix24.com/rest_help/imconnector/setting_line/index.php">documentation</a>)
            /// Открытые линии (<a href="https://dev.1c-bitrix.ru/rest_help/imconnector/setting_line/index.php">документация</a>)
            /// </summary>
            public const string ImOpenLines = "imopenlines";

            /// <summary>
            /// Intranet (<a href="https://training.bitrix24.com/rest_help/application_embedding/index.php#intranet">documentation</a>)<br/>
            /// Интранет (<a href="https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php#intranet">документация</a>)
            /// </summary>
            public const string Intranet = "intranet";

            /// <summary>
            /// Sites (<a href="https://training.bitrix24.com/rest_help/landing/index.php">documentation</a>)<br/>
            /// Сайты (<a href="https://dev.1c-bitrix.ru/rest_help/landing/index.php">документация</a>)
            /// </summary>
            public const string Landing = "landing";

            /// <summary>
            /// Lists (<a href="https://training.bitrix24.com/rest_help/lists/index.php">documentation</a>)<br/>
            /// Списки (<a href="https://dev.1c-bitrix.ru/rest_help/lists/index.php">документация</a>)
            /// </summary>
            public const string Lists = "lists";

            /// <summary>
            /// Activity Stream (<a href="https://training.bitrix24.com/rest_help/log/index.php">documentation</a>)<br/>
            /// Живая лента (<a href="https://dev.1c-bitrix.ru/rest_help/log/index.php">документация</a>)
            /// </summary>
            public const string Log = "log";

            /// <summary>
            /// Mail Service (<a href="https://training.bitrix24.com/rest_help/mail_service/index.php">documentation</a>)<br/>
            /// Почтовые сервисы (<a href="https://dev.1c-bitrix.ru/rest_help/mailservice/index.php">документация</a>)
            /// </summary>
            public const string MailService = "mailservice";

            /// <summary>
            /// Message Service (<a href="https://training.bitrix24.com/rest_help/message_service/index.php">documentation</a><br/>
            /// Служба SMS сообщений (<a href="https://dev.1c-bitrix.ru/rest_help/messageservice/index.php">документация</a>)
            /// </summary>
            public const string MessageService = "messageservice";

            /// <summary>
            /// Mobile application (no documentation in English)<br/>
            /// Мобильное приложение (<a href="https://dev.1c-bitrix.ru/rest_help/mobile/index.php">документация</a>)
            /// </summary>
            public const string Mobile = "mobile";

            /// <summary>
            /// Payment systems (<a href="https://training.bitrix24.com/rest_help/paysystem/index.php">documentation</a>)<br/>
            /// Платёжные системы (<a href="https://dev.1c-bitrix.ru/rest_help/paysystem/index.php">документация</a>)
            /// </summary>
            public const string PaySystem = "pay_system";

            /// <summary>
            /// Application embedding (<a href="https://training.bitrix24.com/rest_help/application_embedding/index.php">documentation</a>)<br/>
            /// Встраивание приложений (<a href="https://dev.1c-bitrix.ru/rest_help/application_embedding/index.php">документация</a>)
            /// </summary>
            public const string Placement = "placement";

            /// <summary>
            /// Pull&amp;Push (<a href="https://training.bitrix24.com/rest_help/pull_push/index.php">documentation</a>)<br/>
            /// Pull&amp;Push (<a href="https://dev.1c-bitrix.ru/rest_help/pull_push/index.php">документация</a>)
            /// </summary>
            public const string Pull = "pull";

            /// <summary>
            /// Robotic process automation (<a href="https://training.bitrix24.com/rest_help/rpa/index.php">documentation</a>)<br/>
            /// Роботизация бизнеса (<a href="https://dev.1c-bitrix.ru/rest_help/rpa/index.php">документация</a>)
            /// </summary>
            public const string Rpa = "rpa";

            /// <summary>
            /// Online store (<a href="https://training.bitrix24.com/rest_help/sale/index.php">documentation</a>)<br/>
            /// Интернет-магазин (<a href="https://dev.1c-bitrix.ru/rest_help/sale/index.php">документация</a>)
            /// </summary>
            public const string Sale = "sale";

            /// <summary>
            /// Workgroups (<a href="https://training.bitrix24.com/rest_help/sonet_group/index.php">documentation</a>)<br/>
            /// Рабочие группы (<a href="https://dev.1c-bitrix.ru/rest_help/socialnetwork/sonet_group/index.php">документация</a>)
            /// </summary>
            public const string SonetGroup = "sonet_group";

            /// <summary>
            /// Tasks (<a href="https://training.bitrix24.com/rest_help/tasks/index.php">documentation</a>)<br/>
            /// Задачи (<a href="https://dev.1c-bitrix.ru/rest_help/tasks/index.php">документация</a>)
            /// </summary>
            public const string Task = "task";

            /// <summary>
            /// Telephony (<a href="https://training.bitrix24.com/rest_help/scope_telephony/index.php">documentation</a>)<br/>
            /// Телефония (<a href="https://dev.1c-bitrix.ru/rest_help/scope_telephony/index.php">документация</a>)
            /// </summary>
            public const string Telephony = "telephony";

            /// <summary>
            /// Time Management (<a href="https://training.bitrix24.com/rest_help/time_management/index.php">documentation</a>)<br/>
            /// Учёт рабочего времени (<a href="https://dev.1c-bitrix.ru/rest_help/timeman/index.php">документация</a>)
            /// </summary>
            public const string Timeman = "timeman";

            /// <summary>
            /// Users (<a href="https://training.bitrix24.com/rest_help/users/index.php">documentation</a>)<br/>
            /// Пользователи (<a href="https://dev.1c-bitrix.ru/rest_help/users/index.php">документация</a>)
            /// </summary>
            public const string User = "user";

            /// <summary>
            /// User agreement (<a href="https://training.bitrix24.com/rest_help/userconsent/index.php">documentation</a>)<br/>
            /// Работа с соглашениями (<a href="https://dev.1c-bitrix.ru/rest_help/userconsent/index.php">документация</a>)
            /// </summary>
            public const string UserConsent = "userconsent";
        }
    }
}
