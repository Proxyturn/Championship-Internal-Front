namespace System.IdentityModel.Tokens
{
    internal class JwtSecurityToken
    {
        private string? authSigningKey;

        public JwtSecurityToken(string? authSigningKey)
        {
            this.authSigningKey = authSigningKey;
        }
    }
}