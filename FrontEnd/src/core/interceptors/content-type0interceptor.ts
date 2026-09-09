import { HttpInterceptorFn } from '@angular/common/http';

export const contentTypeInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.body instanceof FormData) {
    return next(req);
  }

  const clone = req.clone({
    setHeaders: {
      'Content-Type': 'application/json',
    },
  });

  return next(clone);
};
