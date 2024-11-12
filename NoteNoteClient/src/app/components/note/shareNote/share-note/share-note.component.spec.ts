import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShareNoteComponent } from './share-note.component';

describe('ShareNoteComponent', () => {
  let component: ShareNoteComponent;
  let fixture: ComponentFixture<ShareNoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShareNoteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShareNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
