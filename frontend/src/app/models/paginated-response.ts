export interface PaginatedResponse<T> {
    total: number;
    pages: number;
    page: number;
    pageSize: number;
    items: T[];
  }
  