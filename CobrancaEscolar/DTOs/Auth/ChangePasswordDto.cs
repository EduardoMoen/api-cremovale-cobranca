namespace CobrancaEscolar.DTOs.Auth;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

/// <summary>Usado no primeiro acesso: o usuário ainda não possui senha.</summary>
public class SetInitialPasswordDto
{
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
