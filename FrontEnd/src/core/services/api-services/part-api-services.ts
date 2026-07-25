import { inject, Injectable } from '@angular/core';
import { Urls } from '../../constants/urls';
import { HttpClient } from '@angular/common/http';
import { IPart } from '../../interfaces/customize-data.model';

@Injectable({ providedIn: 'root' })
export class PartApiServices {
  // DI
  private readonly http = inject(HttpClient);

  // private
  private readonly url = Urls.apiUrl + '/Part';

  public getAllParts() {
    return this.http.get<IPart[]>(`${this.url}/GetAllParts`);
  }
}
