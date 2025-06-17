import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 100 },
    { duration: '1m', target: 800 },
    { duration: '30s', target: 0 },
  ],
};

export default function () {
  const cursor = '';
  const url = `http://localhost:5000/api/payments/payments/payments-history?cursor=eyJMYXN0SWQiOjIwMDN9&pageSize=20`;
  const params = {
  headers: {
    'Authorization': 'Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJleHAiOjE3NDk1ODg1OTQsImlhdCI6MTc0OTU4NDk5NCwibmJmIjoxNzQ5NTg0OTk0LCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6MywiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiYXJzZW5paS5tZXJrdWxvdkBudXJlLnVhIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpudWxsfQ.hKyVzEk4px-tAwvubEJw0sZN9bAuOD5ispPIij0M8fY',
  },
};
const res = http.get(url, params);

  check(res, {
    'status is 200': (r) => r.status === 200,
    'body is not empty': (r) => r.body && r.body.length > 0,
  });

  sleep(1);
}
