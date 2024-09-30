using System.ComponentModel;

namespace Domain.Const
{
    public enum RankEnum
    {
        [Description("Ratownik")]
        Paramedic = 1,
        [Description("Pielęgniarka")]
        Nurse = 2,
        [Description("Sanitariusz")]
        Sanitarius = 3
    }

    public enum ContractTypeEnum
    {
        [Description("Etat")]
        State = 1,
        [Description("Kontrakt")]
        Contract = 2,
        [Description("Umowa Zlecenie")]
        ContractOfMandate = 3
    }

    public enum VacationTypeEnum
    {
        [Description("Urlop Wypoczynkowy")]
        VacationLeave = 1,

        [Description("Urlop na rządanie")]
        LeaveAtRequest = 2,

        [Description("Zaległy")]
        Arrearage = 3
    }

}
