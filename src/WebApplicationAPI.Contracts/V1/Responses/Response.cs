namespace WebApplicationAPI.Contracts.V1.Responses {
    public class Response<T> {
        public T Data { get; private set; }

        public Response() { }

        public Response(T response) {
            this.Data = response;
        }
    }
}
