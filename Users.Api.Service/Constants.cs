namespace Users.Api.Service;

public static class UserEndPoints
{
    public const string HEALTHCHECKUSERSREADY = "api/v{v:apiVersion}/users/health/ready";
    public const string HEALTHCHECKUSERSLIVE = "api/v{v:apiVersion}/users/health/live";
    public const string ROOTUSERS = "api/v{v:apiVersion}/users";
    public const string USRCREATERECS = "create";
    public const string USRGETALLRECS = "getall";
    public const string USRGETONERECS = "getone";
    public const string USRDELETERECS = "delete";
    public const string USRUPDATERECS = "update";
}

public static class UserMessages
{
    public const string SECURITYDEFINITIONNAME = "Bearer";
    public const string SERVICECOLLECTIONNAME = "Users";
    public const string RATELIMITIRPOLICYNAME = "fixed";
    public const string PASSWORDISREQUIRED = "El campo 'Password' es requerido";
    public const string ALIASISREQUIRED = "El campo 'Alias' es requerido";
    public const string ALIASMINSIZE = "La longitud mímina para el 'Alias' es de 8";
    public const string ALIASMAXSIZE = "La longitud máxima para el 'Alias' es de 16";
    public const string PASSWORDMINSIZE = "La longitud mímina para el 'Password' es de 8";
    public const string PASSWORDMAXSIZE = "La longitud máxima para el 'Password' es de 16";
}

public static class UserVersions
{
    public const double USERSV1 = 1.0;
}