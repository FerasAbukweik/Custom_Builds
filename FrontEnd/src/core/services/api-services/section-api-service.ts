import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Urls } from 'src/core/constants/urls';
import { ISection } from 'src/core/interfaces/customize-data.model';

@Injectable({ providedIn: 'root' })
export class SectionApiServices {
  // private
  private readonly _url = Urls.apiUrl + '/Section';
  private readonly http = inject(HttpClient);

  // api calls
  add(partId: string, title: string) {
    return this.http.post<ISection>(`${this._url}/Add`, { partId, title });
  }
}
