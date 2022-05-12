import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionnaireAnswerFinishComponent } from './questionnaire-answer-finish.component';

describe('QuestionnaireAnswerFinishComponent', () => {
  let component: QuestionnaireAnswerFinishComponent;
  let fixture: ComponentFixture<QuestionnaireAnswerFinishComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionnaireAnswerFinishComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionnaireAnswerFinishComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
