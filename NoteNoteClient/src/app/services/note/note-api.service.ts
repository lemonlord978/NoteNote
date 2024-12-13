import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

interface createNote {
  userId: number;
  title: string;
  content: string;
}

interface createNoteResponse {
  userId: number;
  title: string;
  content: string;
  message: string;
}

interface updateNote {
  noteId: number;
  title: string;
  content: string;
  tags: string[]
}

interface updateNoteResponse {
  noteId: number;
  title: string;
  content: string;
  tags: string[];
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class NoteApiService {

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  private apiUrl = 'http://localhost:5078/api/NoteApi';

  constructor(private http: HttpClient) { }

  getNotes(userId: number, pageNumber: number = 1, pageSize: number = 10, searchQuery: string = ''): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('searchQuery', searchQuery);

    return this.http.get<any>(`${this.apiUrl}/${userId}`, { params });
  }

  getAllNotes(userId: number, searchQuery: string = '', orderBy: string = ""): Observable<any> {
    const params = new HttpParams()
      .set('userId', userId.toString())
      .set('searchQuery', searchQuery)
      .set('$orderby', orderBy);

    return this.http.get<any>(`${this.apiUrl}/`, {
      headers: this.getHeaders(),
      params: params
    });
  }

  create(note: createNote): Observable<createNoteResponse> {
    const data = {
      userId: note.userId,
      title: note.title,
      content: note.content,
    };
    return this.http.post<createNoteResponse>(`${this.apiUrl}/createNote`, data, { headers: this.getHeaders() });
  }

  getNoteById(noteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getNoteById/${noteId}`, { headers: this.getHeaders() });
  }

  shareNote(noteId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/shareNote/${noteId}`, { headers: this.getHeaders() });
  }

  update(note: updateNote): Observable<updateNoteResponse> {
    const data = {
      noteId: note.noteId,
      title: note.title,
      content: note.content,
      tags: note.tags
    };
    return this.http.put<updateNoteResponse>(`${this.apiUrl}/updateNote`, data, { headers: this.getHeaders() });
  }

  delete(noteId: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/deleteNote/${noteId}`, { headers: this.getHeaders() });
  }

  getNewestNote(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/getNewestNote/${userId}`, { headers: this.getHeaders() });
  }
}
