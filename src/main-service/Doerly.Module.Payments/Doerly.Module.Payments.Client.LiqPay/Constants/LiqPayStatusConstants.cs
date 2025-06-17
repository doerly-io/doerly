namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayStatusConstants
{
    #region Загальні статуси

    /// <summary>Неуспішний платіж. Некоректно заповнені дані</summary>
    public const string Error = "error";

    /// <summary>Неуспішний платіж</summary>
    public const string Failure = "failure";

    /// <summary>Платіж повернений</summary>
    public const string Reversed = "reversed";

    /// <summary>Підписка успішно оформлена</summary>
    public const string Subscribed = "subscribed";

    /// <summary>Успішний платіж</summary>
    public const string Success = "success";

    /// <summary>Підписка успішно деактивована</summary>
    public const string Unsubscribed = "unsubscribed";

    #endregion
    
    
    #region Статуси, що потребують підтвердження платежу

    /// <summary>Потрібна 3DS верифікація. Для завершення платежу, потрібно виконати 3ds_verify</summary>
    public const string ThreeDSVerify = "3ds_verify";

    /// <summary>Очікується підтвердження captcha</summary>
    public const string CaptchaVerify = "captcha_verify";

    /// <summary>Потрібне введення CVV картки відправника. Для завершення платежу, потрібно виконати cvv_verify</summary>
    public const string CvvVerify = "cvv_verify";

    /// <summary>Очікується підтвердження дзвінком ivr</summary>
    public const string IvrVerify = "ivr_verify";

    /// <summary>Потрібне OTP підтвердження клієнта. OTP пароль відправлений на номер телефону Клієнта</summary>
    public const string OtpVerify = "otp_verify";

    /// <summary>Очікується підтвердження пароля додатка Приват24</summary>
    public const string PasswordVerify = "password_verify";

    /// <summary>Очікується введення телефону клієнтом</summary>
    public const string PhoneVerify = "phone_verify";

    /// <summary>Очікується підтвердження pin-code</summary>
    public const string PinVerify = "pin_verify";

    /// <summary>Потрібне введення даних одержувача</summary>
    public const string ReceiverVerify = "receiver_verify";

    /// <summary>Потрібне введення даних відправника</summary>
    public const string SenderVerify = "sender_verify";

    /// <summary>Очікується підтвердження в додатку Privat24</summary>
    public const string SenderAppVerify = "senderapp_verify";

    /// <summary>Очікується сканування QR-коду клієнтом</summary>
    public const string WaitQr = "wait_qr";

    /// <summary>Очікується підтвердження оплати клієнтом в додатку Privat24/SENDER</summary>
    public const string WaitSender = "wait_sender";

    #endregion


    #region Інші статуси платежу

    /// <summary>Очікується оплата готівкою в ТСО</summary>
    public const string CashWait = "cash_wait";

    /// <summary>Сума успішно заблокована на рахунку відправника</summary>
    public const string HoldWait = "hold_wait";

    /// <summary>Інвойс створений успішно, очікується оплата</summary>
    public const string InvoiceWait = "invoice_wait";

    /// <summary>Платіж створений, очікується його завершення відправником</summary>
    public const string Prepared = "prepared";

    /// <summary>Платіж обробляється</summary>
    public const string Processing = "processing";

    /// <summary>Кошти з клієнта списані, але магазин ще не пройшов перевірку</summary>
    public const string WaitAccept = "wait_accept";

    /// <summary>Не встановлений спосіб відшкодування у одержувача</summary>
    public const string WaitCard = "wait_card";

    /// <summary>Платіж успішний, буде зарахований в щодобовій проводці</summary>
    public const string WaitCompensation = "wait_compensation";

    /// <summary>Акредитив. Кошти з клієнта списані, очікується підтвердження доставки товару</summary>
    public const string WaitLetterOfCredit = "wait_lc";

    /// <summary>Грошові кошти зарезервовані для повернення за раніше поданою заявкою</summary>
    public const string WaitReserve = "wait_reserve";

    /// <summary>Платіж на перевірці</summary>
    public const string WaitSecure = "wait_secure";

    #endregion
}
