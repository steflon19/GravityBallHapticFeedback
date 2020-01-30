import json
from operator import attrgetter, itemgetter

def read_json(fname):
    with  open(fname) as json_file:
        data = json.load(json_file)
    return data

preData = read_json('./data/survey/pre_survey.json')
postData = read_json('./data/survey/post_survey.json')

unifiedData = []
maxPoints = 0;
for obj in preData:
    unifiedDataEntry = {}
    id = obj['Unique ID']
    obj.pop('Timestamp', None)
    obj.pop('Unique ID', None)
    unifiedDataEntry['id'] = id
    unifiedDataEntry['preSurveyData'] = obj
    projectFileName = './data/project/jsonAll/' + str(id) + '.json'
    projectData = read_json(projectFileName)
    scene = projectData['Scene']
    projectData.pop('Scene', None)
    projectData.pop('ParticipantID', None)
    unifiedDataEntry['sceneType'] = scene
    sumPoints = 0
    for throwEntry in  projectData['ThrowDataEntries']:
        sumPoints += throwEntry['Points']
    if sumPoints > maxPoints:
        maxPoints = sumPoints
    projectData['sumPoints'] = sumPoints
    unifiedDataEntry['projectData'] = projectData
    unifiedData.append(unifiedDataEntry)

# print('max points '  + str(maxPoints))

for obj in postData:
    id = obj['Unique ID']
    obj.pop('Timestamp', None)
    obj.pop('Unique ID', None)
    for obj1 in unifiedData:
        if obj1['id'] == id :
            obj1['postSurveyData'] = obj

# sorted(unifiedData, key=lambda entry: entry['id'])
# sorted(unifiedData, key=lambda entry: entry['sceneType'])
# sorted(unifiedData, key=attrgetter('id', 'sceneType'))
# sorted(unifiedData, key=itemgetter('1', '2')
def byId(e):
    return e['id']

def byScene(e):
    return e['sceneType']

unifiedData.sort(key=byId)
unifiedData.sort(key=byScene)

outS = json.dumps(unifiedData, indent=4, sort_keys=True)
with open('./data/unifiedDataPretty.json', 'w') as outfile:
    outfile.write(outS)
with open('./data/unifiedData.json', 'w') as outfile2:
    json.dump(unifiedData, outfile2)


#processedData = {'id': id, 'preSurveyData': preData, 'postSurveyData': postData, 'projectData': prData};