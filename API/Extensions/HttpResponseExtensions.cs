namespace API.Extensions
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Map business response Code ("00", "400", "500") to standard HTTP status codes.
        /// </summary>
        public static int MapHttpStatusCode(string code)
        {
            return code switch
            {
                "00" => 200,
                "400" => 400,
                "401" => 401,
                "403" => 403,
                "404" => 404,
                "409" => 409,
                "500" => 500,
                _ => 500
            };
        }
    }
}
