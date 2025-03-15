import { HttpParams } from "@angular/common/http";
import { PageInfo } from "../models/page-info";

export function setPaginationParams(pageInfo: PageInfo): HttpParams {
    return new HttpParams()
      .set('PageInfo.Number', pageInfo.number.toString())
      .set('PageInfo.Size', pageInfo.size.toString());
  }