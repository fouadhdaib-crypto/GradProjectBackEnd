using static businessLayer_GP.PostRequestsBusinessLayer;

public class UpdateStatusDto
{
    public int ParticipationID { get; set; }
    public enStatus Status { get; set; }
}