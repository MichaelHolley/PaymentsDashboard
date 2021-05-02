import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tag } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class TagService {

  URL = "api/Tag";

  constructor(private httpClient: HttpClient) { }

  getAllTags(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(this.URL);
  }

  getPrimaryTags(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(this.URL + '/GetPrimaryTags');
  }

  getSecondaryTags(): Observable<Tag[]> {
    return this.httpClient.get<Tag[]>(this.URL + '/GetSecondaryTags');
  }

  createOrUpdateTag(tag: Tag): Observable<any> {
    return this.httpClient.post<Tag>(this.URL, tag);
  }

  deleteTag(id: string): Observable<Tag> {
    return this.httpClient.delete<Tag>(this.URL + '/' + id);
  }
}
