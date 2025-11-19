namespace JffCsharpTools.Domain.Enums
{
    /// <summary>
    /// Enumeration defining JWT (JSON Web Token) parameter types and user claim identifiers.
    /// These values correspond to standard JWT claims and custom application-specific claims
    /// used for token generation, validation, and user identity management.
    /// </summary>
    public enum TokenParameterEnum
    {
        /// <summary>
        /// JWT issuer claim - identifies the principal that issued the token
        /// </summary>
        issuer,

        /// <summary>
        /// JWT audience claim - identifies the recipients that the token is intended for
        /// </summary>
        audience,

        /// <summary>
        /// JWT expiration time claim - identifies the expiration time after which the token must not be accepted
        /// </summary>
        expiration,

        /// <summary>
        /// JWT not before claim - identifies the time before which the token must not be accepted
        /// </summary>
        notBefore,

        /// <summary>
        /// JWT issued at claim - identifies the time at which the token was issued
        /// </summary>
        issuedAt,

        /// <summary>
        /// JWT ID claim - provides a unique identifier for the token
        /// </summary>
        jwtId,

        /// <summary>
        /// JWT subject claim - identifies the principal that is the subject of the token
        /// </summary>
        subject,

        /// <summary>
        /// Custom claim type identifier for application-specific claims
        /// </summary>
        claimType,

        /// <summary>
        /// Custom claim value identifier for application-specific claim values
        /// </summary>
        claimValue,

        /// <summary>
        /// User name claim identifier
        /// </summary>
        name,

        /// <summary>
        /// User email address claim identifier
        /// </summary>
        email,

        /// <summary>
        /// Generic ID claim identifier
        /// </summary>
        id,

        /// <summary>
        /// User ID claim identifier
        /// </summary>
        idUser,

        /// <summary>
        /// JWT subject claim identifier (alternative to 'subject')
        /// </summary>
        sub
    }
}