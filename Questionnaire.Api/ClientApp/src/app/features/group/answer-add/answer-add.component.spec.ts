import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnswerAddComponent } from './answer-add.component';

describe('AnswerAddComponent', () => {
  let component: AnswerAddComponent;
  let fixture: ComponentFixture<AnswerAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnswerAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnswerAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
