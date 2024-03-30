interface IResponse {
  ok: boolean;
}

interface IErrorResponse {
  ok: false;
  code: string;
}

interface ISuccessResponse<TValue> {
  ok: true;
  value: TValue;
}

type ApiResponse<TValue> = IErrorResponse | ISuccessResponse<TValue>;

export default ApiResponse;
