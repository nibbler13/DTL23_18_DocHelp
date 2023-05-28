@extends('layouts.app')

@section('content')
<div class="container">
    <div class="">
        @isset($response)
        @foreach($response as $section)
        <div class="fs-2 fw-bold">{{$section['sectionName']}}</div>

        @foreach($section['groups'] as $group)

        <div class="d-flex fs-4 p-0 "><div>{{$group['group']}}</div><div class="d-block ms-3">{{$group['mkb']}}</div></div>
        <table class="table table-striped mb-5" style="width:100%">
            <thead>
            <tr>
                <th nowrap>№ п/п</th>
                <th nowrap>Тип назначений</th>
                <th nowrap>Назначения</th>
                <th nowrap>Обязательность</th>
                <th nowrap>Критерии исследований/консультаций</th>
            </tr>
            </thead>
            <tbody>
        @foreach($group['services'] as $service)
            <tr>
                <td>{{ $service['order'] }}</td>
                <td>{{ $service['type'] }}</td>
                <td>{{ $service['name'] }}</td>
                <td>{{ $service['necessity'] }}</td>
                <td>{{ $service['comment'] }}</td>
            </tr>
        @endforeach
            </tbody>
        </table>

        @endforeach

        @endforeach
        @endisset
    </div>
</div>
@endsection