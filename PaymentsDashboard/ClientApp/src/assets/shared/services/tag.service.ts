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

  createTag(tag: Tag): Observable<any> {
    return this.httpClient.post<Tag>(this.URL, tag);
  }

  updateTag(tag: Tag): Observable<any> {
    return this.httpClient.put<Tag>(this.URL + '/' + tag.id, tag);
  }

  deleteTag(id: string): Observable<Tag> {
    return this.httpClient.delete<Tag>(this.URL + '/' + id);
  }
}
