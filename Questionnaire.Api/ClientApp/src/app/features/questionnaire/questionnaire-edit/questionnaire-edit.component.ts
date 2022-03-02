import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionnaireService } from '../services/questionnaire.service';

@Component({
  selector: 'app-questionnaire-edit',
  templateUrl: './questionnaire-edit.component.html',
  styleUrls: ['./questionnaire-edit.component.css']
})
export class QuestionnaireEditComponent implements OnInit {
  questionnaireForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  questionnaireId: number;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private questionnaireService: QuestionnaireService) { }

    ngOnInit() {
      this.questionnaireForm = this.formBuilder.group({
        title: ['', Validators.required],
        description: ['', Validators.required],
        begining: ['', Validators.required],
        finish: ['', Validators.required],
        visibleToGroup: false
      });
      this.route.params.subscribe(params => {
        this.loadQuestionnaire(params['id']);
        this.questionnaireId = params['id'];
      });
    }

    private loadQuestionnaire(id: number) {
      this.questionnaireService.getById(id)
      .subscribe(result => {
        this.questionnaireForm.patchValue({
          title: result.title,
          description: result.description,
          begining: formatDate(result.begining, 'yyyy-MM-ddThh:mm', 'en_US'),
          finish: formatDate(result.finish, 'yyyy-MM-ddThh:mm', 'en_US'),
          visibleToGroup: result.visibleToGroup
        });
      });
    }

    get form() {
      return this.questionnaireForm.controls;
    }

    onSubmit() {
      this.submitted = true;

      if (this.questionnaireForm.invalid) {
        return;
      }

      this.loading = true;
      this.questionnaireService.update(this.questionnaireId, this.questionnaireForm.value)
        .subscribe(
          result => {
            this.router.navigate(['/questionnaire', this.questionnaireId]);
          },
          error => {
            this.error = error;
            console.error(error);
            this.loading = false;
        });
    }

}
