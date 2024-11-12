import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

interface createNote {
  userId: number;
  title: string;
  content: string;
}

interface updateNote {
  noteId: number;
  title: string;
  content: string;
  tags: string[]
}

@Injectable({
  providedIn: 'root'
})
export class NoteApiService {

  private apiUrl = 'http://localhost:5078/api/NoteApi';

  constructor(private http: HttpClient) { }

  // note-api.service.ts
  getNotes(userId: number, pageNumber: number = 1, pageSize: number = 10, searchQuery: string = ''): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('searchQuery', searchQuery);
  
    return this.http.get<any>(`${this.apiUrl}/${userId}`, { params });
  }

  create(note: createNote): Observable<createNote> {
    const data = {
      userId: note.userId,
      title: note.title,
      content: note.content,
    };
    return this.http.post<createNote>(`${this.apiUrl}/createNote`, data);
  }

  getNoteById(noteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getNoteById/${noteId}`);
  }

  update(note: updateNote): Observable<updateNote> {
    const data = {
      noteId: note.noteId,
      title: note.title,
      content: note.content,
      tags: note.tags
    };
    return this.http.put<updateNote>(`${this.apiUrl}/updateNote`, data);
  }

  delete(noteId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/deleteNote/${noteId}`);
  }

  getNewestNote(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getNewestNote/${userId}`);
  }
}
