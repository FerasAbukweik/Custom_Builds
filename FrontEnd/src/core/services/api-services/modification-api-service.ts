import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Urls } from 'src/core/constants/urls';
import { IModification, ISection } from 'src/core/interfaces/customize-data.model';
import { ModificationAddDTO } from 'src/core/DTO/modification-add-dto';

@Injectable({ providedIn: 'root' })
export class ModificationApiServices {
  // private
  private readonly _url = Urls.apiUrl + '/Modifications';
  private readonly http = inject(HttpClient);

  // api calls
  add(newModData: ModificationAddDTO) {
    let formData = new FormData();

    formData.append('name', newModData.name);
    formData.append('image', newModData.image, newModData.image.name);
    formData.append('price', newModData.price.toString());
    formData.append('sectionId', newModData.sectionId);

    return this.http.post<IModification>(`${this._url}/Add`, formData);
  }

  remove(modId: string) {
    return this.http.delete(`${this._url}/Remove/${modId}`);
  }
}
